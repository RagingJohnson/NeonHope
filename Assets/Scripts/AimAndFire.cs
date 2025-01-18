using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAndFire : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] GameObject Bullet;

    [SerializeField] float RateOfFire = 1f;
    private float CooldownRemaining = 0;
    private Vector3 Direction = Vector3.up;

    private void Update()
    {
        LookAtTarget();
        Cooldown();
    }

    private void Fire()
    {
        CreateBullet();
    }

    private void Cooldown()
    {
        CooldownRemaining -= Time.deltaTime;
        if (CooldownRemaining < 0)
        {
            CooldownRemaining = RateOfFire;
            Fire();
        }
    }

    private void CreateBullet()
    {
        Quaternion bulletRotation = Quaternion.Euler(0, 0, 90);
        GameObject newBullet = Instantiate(Bullet, transform.position, bulletRotation);
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        newBullet.GetComponent<BulletMovement>().Initialize(direction, transform.rotation * bulletRotation);
    }

    private void LookAtTarget()
    {
        Vector3 direction = Target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
