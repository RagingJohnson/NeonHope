using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    Vector3 Direction = Vector3.zero;
    float MoveSpeed = 1f;

    void FixedUpdate()
    {
        transform.position += Direction * MoveSpeed * Time.fixedDeltaTime;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public void SetMoveSpeed(float speed)
    {
        MoveSpeed = speed;
    }

    public void Initialize(Vector3 direction, Quaternion rotation)
    {
        SetDirection(direction);
        SetRotation(rotation);
    }

    public void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
