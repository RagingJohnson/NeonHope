using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    float CurrentScale = 1f;
    [SerializeField] float MaxScale = 2f;
    [SerializeField] float MinScale = 0.25f;
    float Direction = 1f;
    [SerializeField] float RateOfRescale = 0.3f;
    Vector3 OriginalScale;
    Gravity GravityObject;
    float originalGravitationalPullValue;

    void Start()
    {
        OriginalScale = transform.localScale;
        GravityObject = GameObject.Find("Gravity").GetComponent<Gravity>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentScale > MaxScale)
        {
            Direction = -1f;
        }
        else if(CurrentScale < MinScale)
        {
            Direction = 1f;
        }

        CurrentScale += RateOfRescale * Direction * Time.deltaTime;

        transform.localScale = OriginalScale * CurrentScale;

        GravityObject.UpdateScaleFactor(CurrentScale);
    }
}
