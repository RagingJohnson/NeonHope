using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithParent : MonoBehaviour
{
    void Update()
    {
        transform.localScale = GetComponentInParent<Transform>().localScale;
    }
}
