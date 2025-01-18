using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseObject : MonoBehaviour
{
    [SerializeField]GameObject Target;
    [SerializeField] float MoveSpeed = 2f;

    private void Update()
    {
        MoveTowardTarget();
    }

    private bool MoveTowardTarget()
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        transform.position += direction * MoveSpeed * Time.deltaTime;

        return (Target.transform.position - transform.position).magnitude > 1.1f * MoveSpeed;
    }
}
