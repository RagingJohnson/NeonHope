using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float initialRotation = 0;
    [SerializeField] int rotationRate = 1;
    void Start()
    {
        transform.Rotate(0, 0, initialRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * rotationRate);
    }
}
