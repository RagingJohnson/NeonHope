using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour
{
    // Start is called before the first frame update
    List<Detonation> ActiveDetonations = new List<Detonation>();
    List<Detonation> DeadDetonations = new List<Detonation>();
    [SerializeField] Detonation DetonationPrefab;
    [SerializeField] float DetonationMaxCooldown = 3f;
    float DetonationCooldown = 0;
    void Start()
    {
        //Position the detonation
        DetonationCooldown = DetonationMaxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDetonations();

        CleanUpDetonations();
    }

    public void CreateDetonation()
    {
        Detonation newDetonation = Instantiate(DetonationPrefab).GetComponent<Detonation>();
        newDetonation.transform.position = transform.position;
        ActiveDetonations.Add(newDetonation);
    }

    public void CreateDetonation(float expansionRate, float maxScale)
    {
        Detonation newDetonation = Instantiate(DetonationPrefab).GetComponent<Detonation>();
        newDetonation.ExpansionRate = expansionRate;
        newDetonation.MaxScale = maxScale;
        newDetonation.transform.position = transform.position;
        ActiveDetonations.Add(newDetonation);
    }

    void UpdateDetonations()
    {
        foreach (Detonation detonation in ActiveDetonations)
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
        foreach (Detonation detonation in DeadDetonations)
        {
            ActiveDetonations.Remove(detonation);
            Object.Destroy(detonation.gameObject);
        }

        DeadDetonations.Clear();
    }
}
