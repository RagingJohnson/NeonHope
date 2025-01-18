using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] float GravitationalConstant = 0.0001f;
    Vector2 _centreOfGravity;
    float _scaleFactor = 1f;

    private void Start()
    {
        _centreOfGravity = (Vector2) transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _centreOfGravity = (Vector2)transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Control>().AddGravity(_centreOfGravity, GravitationalConstant * _scaleFactor * _scaleFactor * _scaleFactor);
        }
    }

    public void UpdateScaleFactor(float newScaleFactor)
    {
        _scaleFactor = newScaleFactor;
    }
}
