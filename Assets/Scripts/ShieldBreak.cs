using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBreak : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Detonation"))
        {
            Destroy(this.gameObject);
        }
    }
}
