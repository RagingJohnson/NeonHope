using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField]List<GameObject> Points = new List<GameObject>();
    int CurrentPoint;
    float MoveSpeed = 2f;

    private void Update()
    {
        if(MoveToPoint(Points[CurrentPoint].transform.position))
        {
            SwitchToNextPoint();
        }
    }

    private bool MoveToPoint(Vector3 pointPosition)
    {
        Vector3 direction = (pointPosition - transform.position).normalized * MoveSpeed * Time.deltaTime;
        transform.position += direction;
        return (pointPosition - transform.position).magnitude < 1.1 * MoveSpeed;
    }

    private void SwitchToNextPoint()
    {
        ++CurrentPoint;
        CurrentPoint %= Points.Count;
    }
}
