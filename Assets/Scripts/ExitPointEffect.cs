using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPointEffect : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject ShimmerWavePrefab;
    Renderer componentRenderer;

    [SerializeField] float ShimmerMaxCooldown = 0.1f;
    private float ShimmerCooldown;

    List<Shimmer> ActiveShimmers = new List<Shimmer>();
    List<Shimmer> DeadShimmers = new List<Shimmer>();
    // Start is called before the first frame update
    void Start()
    {
        ShimmerCooldown = ShimmerMaxCooldown;
    }

    private void Awake()
    {
        componentRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (componentRenderer.isVisible)
        {
            ShimmerActive();
        }
        else
        {
            ShimmerInactive();
        }
    }

    void ShimmerActive()
    {
        UpdateShimmers();

        if (ShimmerCooldown > 0f)
        {
            ShimmerCooldown -= Time.deltaTime;
            return;
        }

        Shimmer newShimmer = Instantiate(ShimmerWavePrefab).GetComponent<Shimmer>();

        newShimmer.transform.position = transform.position;
        ActiveShimmers.Add(newShimmer);

        ShimmerCooldown = ShimmerMaxCooldown;
    }

    void UpdateShimmers()
    {
        foreach (Shimmer shim in ActiveShimmers)
        {
            if(!shim.Alive)
            {
                DeadShimmers.Add(shim);
            }
        }

        DestroyDeadShimmers();
    }

    void ShimmerInactive()
    {
        if (ActiveShimmers.Count > 0)
        {
            foreach (Shimmer shim in ActiveShimmers)
            {
                DeadShimmers.Add(shim);
            }
        }

        DestroyDeadShimmers();
    }

    void DestroyDeadShimmers()
    {
        if (DeadShimmers.Count > 0)
        {
            foreach (Shimmer shim in DeadShimmers)
            {
                ActiveShimmers.Remove(shim);
                Object.Destroy(shim.gameObject);
            }

            DeadShimmers.Clear();
        }
    }
}
