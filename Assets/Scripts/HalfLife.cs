using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds a time limit to an object which, when reached, will split the object into two smaller versions of that object.
/// </summary>
public class HalfLife : MonoBehaviour
{
    [SerializeField] GameObject HalfLifePrefab;
    [SerializeField] float startingCooldown = 1f;
    float cooldownCount;

    private void Start()
    {
        cooldownCount = startingCooldown;
    }

    void Update()
    {
        if (cooldownCount > 0f)
        {
            cooldownCount -= Time.deltaTime;
        }
    }

    public void Split()
    {
        CreateNewHalfLife();
        CreateNewHalfLife();

        Destroy(this);
        this.gameObject.SetActive(false);
    }

    void CreateNewHalfLife()
    {
        GameObject newHalfLife = Instantiate(HalfLifePrefab);
        newHalfLife.GetComponent<HalfLife>().Reset();
        newHalfLife.GetComponent<Movement>().Reset();
        newHalfLife.transform.localScale = transform.localScale * 0.75f;
        newHalfLife.GetComponent<Rigidbody2D>().mass *= 0.75f;
    }

    void Reset()
    {
        cooldownCount = startingCooldown;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Detonation"))
        {
            if (transform.localScale.magnitude > .1f && cooldownCount <= 0f)
            {
                Split();
            }
        }
    }
}
