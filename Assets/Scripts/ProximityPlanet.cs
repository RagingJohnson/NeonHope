using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityPlanet : MonoBehaviour
{
    // Start is called before the first frame update
    Proximity _proximity;
    void Start()
    {
        _proximity = this.gameObject.GetComponentInChildren<Proximity>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _proximity.ResetTime();
            _proximity.FreezeTime();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _proximity.UnfreezeTime();
    }
}