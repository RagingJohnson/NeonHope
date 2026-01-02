using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    private bool transition = false;
    [SerializeField] float Countdown = 1.5f;

    private void Awake()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.GetComponent<SpriteRenderer>().color = Color.black;
            transition = true;
        }
    }

    private void Update()
    {
        if (!transition)
        {
            return;
        }

        if (Countdown > 0f)
        {
            Countdown -= Time.deltaTime;
            return;
        }

        //Transition to next scene.
        GameManager.TransitionToNextScene();
    }
}