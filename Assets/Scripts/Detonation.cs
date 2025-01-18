using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonation : MonoBehaviour
{
    // Start is called before the first frame update
    float scale = 0;
    [SerializeField] float ExpansionRate = .2f;
    [SerializeField] float MaxScale = .2f;
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
