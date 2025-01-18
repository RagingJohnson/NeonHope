using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> ActiveDetonations = new List<GameObject>();
    List<GameObject> DeadDetonations = new List<GameObject>();
    [SerializeField] GameObject DetonationPrefab;
    [SerializeField] float DetonationMaxCooldown = 3f;
    float DetonationCooldown;
    void Start()
    {
        //Position the detonation
        DetonationCooldown = DetonationMaxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //Trigger the detonation
        UpdateDetonations();

        CleanUpDetonations();
    }

    public void CreateDetonation()
    {
        /*
        if(DetonationCooldown < DetonationMaxCooldown)
        {
            DetonationCooldown = DetonationCooldown <= 0 ? DetonationMaxCooldown : DetonationCooldown - Time.deltaTime;
            return;
        }
        */

        GameObject newDetonation = Instantiate(DetonationPrefab);
        newDetonation.transform.position = transform.position;
        ActiveDetonations.Add(newDetonation);
    }

    void UpdateDetonations()
    {
        foreach (GameObject detonation in ActiveDetonations)
        {
            if (!detonation.GetComponent<Detonation>().Alive)
            {
                DeadDetonations.Add(detonation);
            }
            else
            {
                detonation.transform.position = transform.position;
            }
        }
    }

    void CleanUpDetonations()
    {
        foreach (GameObject detonation in DeadDetonations)
        {
            ActiveDetonations.Remove(detonation);
            Destroy(detonation);
        }

        DeadDetonations.Clear();
    }
}
