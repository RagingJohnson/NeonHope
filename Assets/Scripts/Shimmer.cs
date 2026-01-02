using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shimmer : MonoBehaviour
{
    float scale = 0;
    [SerializeField] public float ExpansionRate = .2f;
    [SerializeField] public float MaxScale = .2f;
    public bool Alive = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        scale += Time.deltaTime * ExpansionRate;
        transform.localScale = Vector3.one * scale;

        if (scale >= MaxScale)
        {
            Alive = false;
        }
    }
}