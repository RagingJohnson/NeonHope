using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> allObjects;
    [SerializeField] float graviationalPull = 2;
    void Start()
    {
        List<GameObject> removeObjects = new List<GameObject>();
        allObjects = FindObjectsOfType<GameObject>().ToList<GameObject>();
        Rigidbody2D outputRigidBody;

        foreach (GameObject obj in allObjects)
        {
            if (obj == gameObject || !obj.TryGetComponent<Rigidbody2D>(out outputRigidBody))
            {
                removeObjects.Add(obj);
            }
        }

        foreach (GameObject obj in removeObjects)
        {
            allObjects.Remove(obj);
        }

        removeObjects.Clear();
    }

    void BlackHolePull()
    {
        foreach(GameObject obj in allObjects)
        {
            float objMass = obj.GetComponent<Rigidbody2D>().mass;
            Vector2 objPosition = (Vector2)obj.transform.position;

            Vector2 differenceVector = (Vector2)transform.position - objPosition;
            Vector2 directionVector = differenceVector.normalized;

            float distance = differenceVector.magnitude;
            float force = graviationalPull / Mathf.Pow(distance, 2);

            Vector2 gravitationalPull = directionVector * force * Time.deltaTime;
            objPosition += gravitationalPull;

            obj.transform.position = objPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        BlackHolePull();
    }
}