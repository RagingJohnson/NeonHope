using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserJunction : MonoBehaviour
{
    public enum JunctionColour { Cyan, Mangenta, Yellow };
    [SerializeField] public JunctionColour junctionColour;
    void Start()
    {
        switch (junctionColour)
        {
            case JunctionColour.Cyan:
                LaserIntersection.CyanJunction = gameObject.GetComponent<LaserJunction>();
                break;
            case JunctionColour.Mangenta:
                LaserIntersection.MagentaJunction = gameObject.GetComponent<LaserJunction>();
                break;
            case JunctionColour.Yellow:
                LaserIntersection.YellowJunction = gameObject.GetComponent<LaserJunction>();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdatePosition(Vector2 newPos)
    {
        transform.position = newPos;
    }
}