using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System;

public class Movement : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 1f;
    [SerializeField] Vector2 MovementDirection = new Vector2(1f, 0f);
    float WaveValue = 0f;
    [SerializeField] float SpiralMagnitude = 1f;
    bool SpiralIncreasing = true;
    [SerializeField] GameObject OrbitTarget;
    float AngluarRotationCoefficient = 90f;
    [SerializeField] float DistanceFromTarget = 1f;
    [SerializeField] float OscillateDistance = 5f;
    Vector3 StartPosition = Vector3.zero;
    [SerializeField] float CircleSize = 1f;

    [SerializeField] bool HasBeenRandomized = false;

    Rigidbody2D Rigidbody;

    enum MovementType {Circular, OscillateVertical, OscillateHorizontal, Spiral, RelativePosition, Orbit, SimpleMovement, Stationary, Random};
    [SerializeField] MovementType movementType = MovementType.Circular;

    private void Start()
    {
        StartPosition = transform.position;
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    public void Reset()
    {
        HasBeenRandomized = false;
    }

    public void SetMovementDirection(Vector2 newDirection)
    {
        MovementDirection = newDirection;
    }

    void RandomDirection()
    {
        if (!HasBeenRandomized)
        {
            float randomX = UnityEngine.Random.Range(-1f, 1f);
            float randomY = UnityEngine.Random.Range(-1f, 1f);
            float randomZ = UnityEngine.Random.Range(-60f, 60f);
            MovementDirection = new Vector2(randomX, randomY).normalized;
            HasBeenRandomized = true;

            RandomizeRotation(randomZ);
        }

        SimpleMovement();
    }

    void SetPositionRelativeToObject(GameObject targetObject)
    {
        transform.position = new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + DistanceFromTarget, 0);
    }

    void RandomizeRotation(float randomZ)
    {
        Rigidbody.AddTorque(randomZ);
    }   

    void CircularMovement()
    {
        WaveValue += MovementSpeed * Time.deltaTime;
        WaveValue %= 360f;

        Vector2 direction = new Vector2(
            -math.cos(WaveValue),
            math.sin(WaveValue)) * CircleSize;

        transform.position = (Vector2)transform.position + (direction.normalized * MovementSpeed * Time.deltaTime) * CircleSize;
    }
    
    void OscillateMovement()
    {
        SimpleMovement();
        if ((transform.position - StartPosition).magnitude > OscillateDistance)
        {
            MovementDirection *= -1;
        }
    }

    void SimpleMovement()
    {
        transform.position = new Vector2(
            transform.position.x + (MovementDirection.x * MovementSpeed * Time.deltaTime),
            transform.position.y + (MovementDirection.y * MovementSpeed * Time.deltaTime));
    }

    void SpiralMovement()
    {
        WaveValue += MovementSpeed * Time.deltaTime;
        WaveValue %= 360f;

        Vector3 direction = new Vector3(
            -math.cos(WaveValue),
            math.sin(WaveValue),
            0);

        transform.position = transform.position + (direction.normalized * MovementSpeed * Time.deltaTime) * SpiralMagnitude;
        SpiralControl();
    }

    void Move()
    {
        switch (movementType)
        {
            case MovementType.Circular:
                CircularMovement();
                break;
            case MovementType.Spiral:
                SpiralMovement();
                break;
            case MovementType.OscillateVertical:
                OscillateMovement();
                break;
            case MovementType.OscillateHorizontal:
                OscillateMovement();
                break;
            case MovementType.RelativePosition:
                SetPositionRelativeToObject(OrbitTarget);
                movementType = MovementType.Orbit;
                break;
            case MovementType.Orbit:
                transform.RotateAround(OrbitTarget.transform.position, Vector3.forward, AngluarRotationCoefficient * MovementSpeed * Time.deltaTime);
                    break;
            case MovementType.SimpleMovement:
                SimpleMovement();
                break;
            case MovementType.Random:
                RandomDirection();
                break;
            case MovementType.Stationary:
                break;
            default:
                break;
        }
    }

    void SpiralControl()
    {
        SpiralMagnitude += SpiralIncreasing ? 0.001f : -0.001f;
        SpiralIncreasing = SpiralMagnitude >= 5f ? false : SpiralMagnitude <= 0.1f ? true : SpiralIncreasing;
        SpiralMagnitude = Mathf.Clamp(SpiralMagnitude, 0.1f, 5f);
    }
}