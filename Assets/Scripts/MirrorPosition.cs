using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPosition : MonoBehaviour
{
    [SerializeField] GameObject ObjectToMirror;

    Vector2 cameraCentrePosition;
    Vector2 positionVectorFromCameraCentre;
    // Start is called before the first frame update
    void Start()
    {
        cameraCentrePosition = (Vector2)Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        positionVectorFromCameraCentre = -cameraCentrePosition + (Vector2)ObjectToMirror.transform.position;
        transform.position = -positionVectorFromCameraCentre;
    }
}
