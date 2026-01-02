using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float maxDistanceRay = 40;
    private float currentDistanceRay;
    [SerializeField] Material collisionMaterial;
    public enum LaserColour { Blue, Red, Green };
    [SerializeField] public LaserColour colour;

    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    Transform m_transform;
    Transform p_transform;

    public BoxCollider2D m_boxCollider;

    Material native_material;

    RaycastHit2D rayCastHit;

    int CurrentLaserCollisionsTotal = 0;
    void Start()
    {
        currentDistanceRay = maxDistanceRay;
    }

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        p_transform = GetComponentInParent<Transform>();
        m_boxCollider = GetComponent<BoxCollider2D>();
        native_material = m_lineRenderer.material;

        Physics.queriesHitTriggers = true;
        Physics2D.queriesHitTriggers = true;
    }

    void ShootLaser()
    {
        Vector2 direction = p_transform.up;
        Vector2 startPos = laserFirePoint.position;
        Vector2 endPos = startPos + direction * currentDistanceRay;

        Draw2DRay(startPos, endPos);
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        // Set LineRenderer positions in world space
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);

        // Convert world space points to local space for the Collider
        Vector2 localStart = p_transform.InverseTransformPoint(startPos);
        Vector2 localEnd = p_transform.InverseTransformPoint(endPos);

        // Update BoxCollider2D to match the laser
        if (m_boxCollider != null)
        {
            // Set the box collider to be a thin line along the laser
            float length = Vector2.Distance(localStart, localEnd);

            m_boxCollider.size = new Vector2(0.01f, length);  // Thin box, length along x-axis
            m_boxCollider.offset = (localStart + localEnd) / 2;  // Center of the line

            // Set the rotation to match the parent's rotation
            m_boxCollider.transform.rotation = p_transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        ShootLaser();
    }

    public void ChangeMaterial(Material newMaterial)
    {
        m_lineRenderer.material = newMaterial;
    }

    public void ResetMaterial()
    {
        m_lineRenderer.material = native_material;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 size = new Vector2(0f, maxDistanceRay);

        if (collision.gameObject.CompareTag("Laser"))
        {
            //ChangeMaterial(collisionMaterial);

            rayCastHit = Physics2D.Raycast(laserFirePoint.position, p_transform.up);

            ++CurrentLaserCollisionsTotal;
            ++LaserIntersection.TotalLaserCollisions;

            if (CurrentLaserCollisionsTotal > 0)
            {

            }
        }
        else
        {
            // Your regular collision handling code here
        }
    }

    private Vector2 CollideWithLaser(Collider2D collision, string[] laserMaskLabel)
    {
        Vector2 direction = p_transform.up;
        Vector2 startPos = laserFirePoint.position;

        bool wasTrigger = collision.isTrigger;
        collision.isTrigger = false;

        // Raycast to get the actual intersection point
        RaycastHit2D preciseHit = Physics2D.Raycast(startPos, direction, maxDistanceRay, ~(LayerMask.GetMask(laserMaskLabel)));

        // Restore the trigger setting
        collision.isTrigger = wasTrigger;

        // Check if the raycast hit a valid point
        if (preciseHit.collider != null)
        {
            return preciseHit.point;
        }
        else
        {
            // Return end point of laser
            return collision.gameObject.transform.position; 
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        string[] laserMaskLabel = colour == LaserColour.Green ? new string[] { "GreenLaser", "RedLaser", "Player", "Exit" }
            : colour == LaserColour.Blue ? new string[] { "BlueLaser", "GreenLaser", "Player", "Exit" }
            : new string[] {"RedLaser", "BlueLaser", "Player", "Exit" };

        Vector2 pointOfCollision = CollideWithLaser(collision, laserMaskLabel);

        if (collision.gameObject.CompareTag("Laser"))
        {
            switch (colour)
            {
                case LaserColour.Blue:
                    if (collision.GetComponent<Laser>().colour == LaserColour.Red)
                    {
                        LaserIntersection.MagentaJunction.UpdatePosition(pointOfCollision);
                        LaserIntersection.MagentaJunctionActive = true;
                    }
                    break;
                case LaserColour.Green:
                    if (collision.GetComponent<Laser>().colour == LaserColour.Blue)
                    {
                        LaserIntersection.CyanJunction.UpdatePosition(pointOfCollision);
                        LaserIntersection.CyanJunctionActive = true;
                    }
                    break;
                case LaserColour.Red:
                    if (collision.GetComponent<Laser>().colour == LaserColour.Green)
                    {
                        LaserIntersection.YellowJunction.UpdatePosition(pointOfCollision);
                        LaserIntersection.YellowJunctionActive = true;
                    }
                    break;
            }
        }
        else if(!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Exit"))
        {
            currentDistanceRay = (-(Vector2)laserFirePoint.position + pointOfCollision).magnitude;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentDistanceRay = maxDistanceRay;

        if (collision.gameObject.CompareTag("Laser"))
        {
            string laserMaskLabel = colour == LaserColour.Green ? "GreenLaser"
            : colour == LaserColour.Blue ? "BlueLaser"
            : "RedLaser";

            switch (laserMaskLabel)
            {
                case "BlueLaser":
                    if (collision.GetComponent<Laser>().colour == LaserColour.Red)
                    {
                        LaserIntersection.MagentaJunction.UpdatePosition(collision.gameObject.transform.position);
                        LaserIntersection.MagentaJunctionActive = false;
                    }
                    break;
                case "GreenLaser":
                    if (collision.GetComponent<Laser>().colour == LaserColour.Blue)
                    {
                        LaserIntersection.CyanJunction.UpdatePosition(collision.gameObject.transform.position);
                        LaserIntersection.CyanJunctionActive = false;
                    }
                    break;
                case "RedLaser":
                    if (collision.GetComponent<Laser>().colour == LaserColour.Green)
                    {
                        LaserIntersection.YellowJunction.UpdatePosition(collision.gameObject.transform.position);
                        LaserIntersection.YellowJunctionActive = false;
                    }
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}