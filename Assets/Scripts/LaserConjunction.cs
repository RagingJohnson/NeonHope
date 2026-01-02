using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserConjunction : MonoBehaviour
{
    Vector2 ExitPointOrigin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        ExitPointOrigin = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (LaserIntersection.CyanJunction != null && LaserIntersection.CyanJunctionActive &&
            LaserIntersection.MagentaJunction != null && LaserIntersection.MagentaJunctionActive &&
            LaserIntersection.YellowJunction != null && LaserIntersection.YellowJunctionActive)
        {
            if ((LaserIntersection.CyanJunction.transform.position - LaserIntersection.MagentaJunction.transform.position).magnitude < 0.2f &&
                (LaserIntersection.MagentaJunction.transform.position - LaserIntersection.YellowJunction.transform.position).magnitude < 0.2f &&
                (LaserIntersection.YellowJunction.transform.position - LaserIntersection.CyanJunction.transform.position).magnitude < 0.2f)
            {
                Vector2 midpoint = (LaserIntersection.CyanJunction.transform.position + LaserIntersection.MagentaJunction.transform.position + LaserIntersection.YellowJunction.transform.position) / 3;

                gameObject.transform.position = new Vector3(midpoint.x, midpoint.y, 0f);
            }
            else
            {
                gameObject.transform.position = ExitPointOrigin;
            }
        }
    }
}