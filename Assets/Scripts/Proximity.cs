using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    Gradient Gradient = new Gradient();
    [SerializeField] float MaximumTime = 10f;
    private float CurrentTime = 0;
    private bool InProximity = false;
    private bool TimeFrozen = false;

    GameObject Parent;
    private void Start()
    {
        var colours = new GradientColorKey[3];
        colours[0] = new GradientColorKey(Color.red, 0.0f);
        colours[1] = new GradientColorKey(Color.yellow, 0.5f);
        colours[2] = new GradientColorKey(Color.green, 1.0f);

        Gradient.SetKeys(colours, new GradientAlphaKey[0]);

        Parent = transform.parent.gameObject;
        Parent.GetComponent<SpriteRenderer>().color = Color.black;
    }

    private void Update()
    {
        if(!InProximity)
        {
            CurrentTime -= Time.deltaTime * 2;
            CurrentTime = Mathf.Clamp(CurrentTime, 0f, MaximumTime);
        }

        if(CurrentTime <= 0)
        {
            Parent.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else if (CurrentTime < MaximumTime)
        {
            Parent.GetComponent<SpriteRenderer>().color = Gradient.Evaluate(CurrentTime / MaximumTime);
        }
        else
        {
            Parent.GetComponent<SpriteRenderer>().color = Color.white;
            Parent.GetComponent<CircleCollider2D>().enabled = false;
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InProximity = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(TimeFrozen)
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            CurrentTime += UnityEngine.Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            InProximity = false;
        }
    }

    public void ResetTime()
    {
        CurrentTime = 0f;
    }

    public void FreezeTime()
    {
        TimeFrozen = true;
    }

    public void UnfreezeTime()
    {
        TimeFrozen = false;
    }
}