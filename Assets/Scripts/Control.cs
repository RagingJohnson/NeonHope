using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// Used to control the player, both through gravitational pull, and User input.
/// </summary>
public class Control : MonoBehaviour
{
    [SerializeField] Vector2 Direction = Vector2.up;
    [SerializeField] float BaseSpeed = 1.5f;
    [SerializeField] float MaxSpeed = 5f;
    [SerializeField] float DecelerationRate = -1f;
    [SerializeField] float AccelerationRate = 2f;
    [SerializeField] float Speed = 1.5f;
    [SerializeField] float PlayerGravity = 1.5f;
    Vector2 _newDirection;
    SpriteRenderer _spriteRenderer;

    Vector2 _oldPosition;
    List<Vector2> _listOfGravitationalPulls = new List<Vector2>();

    bool _isAffectedByGravity = true;

    List<GameObject> ActiveDetonations = new List<GameObject>();
    List<GameObject> DeadDetonations = new List<GameObject>();
    [SerializeField] GameObject DetonationPrefab;
    [SerializeField] int maximumDetonationCharges = 30;

    private void Awake()
    {
        _oldPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Controls();
        Vector2 newPosition = (Vector2)transform.position + (Direction * Speed * Time.fixedDeltaTime);

        if (GetComponent<Renderer>().isVisible)
        {
            _newDirection = newPosition - _oldPosition;
        }

        if (_newDirection.magnitude > 0f)
        {
            Direction = _newDirection.normalized;
            _oldPosition = (Vector2)transform.position;
        }

        transform.position = newPosition + GetSumOfGravity();

        MaintainDetonations();

        AddEffects();
    }

    /// <summary>
    /// Keeps track of, updates, and removes detonations as they progress through their lifecyles.
    /// </summary>
    void MaintainDetonations()
    {
        UpdateDetonations();
        CleanUpDetonations();
    }

    /// <summary>
    /// Adds any VFX related to the Player being in certain states.
    /// </summary>
    public void AddEffects()
    {
        _spriteRenderer.color = _isAffectedByGravity ? Color.green : Color.yellow;
    }

    /// <summary>
    /// Add the effects of gravity from a gravitational body, such as a planet or a star.
    /// </summary>
    /// <param name="gravitationalCentre"></param>
    /// <param name="gravitationalConstant"></param>
    public void AddGravity(Vector2 gravitationalCentre, float gravitationalConstant)
    {
        if (!_isAffectedByGravity)
        {
            return;
        }

        Vector2 differenceVector = gravitationalCentre - (Vector2)transform.position;
        Vector2 directionVector = differenceVector.normalized;

        float distance = differenceVector.magnitude;

        if(distance < 1f) 
        {
            distance = 1f;
        }

        float force = (gravitationalConstant * PlayerGravity) / math.pow(distance, 2);

        Vector2 gravitationalPull = directionVector * force * Time.deltaTime;
        _listOfGravitationalPulls.Add(gravitationalPull);
    }

    /// <summary>
    /// If the Player is currently affected by gravity, calculate the net effect of all gravitaional forces on them.
    /// </summary>
    /// <returns></returns>
    public Vector2 GetSumOfGravity()
    {
        return _isAffectedByGravity ? CalculateGravity() : Vector2.zero;
    }

    /// <summary>
    /// Triggers the functions based on which Payer controls are active.
    /// </summary>
    public void Controls()
    {
        Accelerate(Input.GetKey(GameManager.controls["Accelerate"]));
        _isAffectedByGravity = !Input.GetKey(GameManager.controls["Escape"]);
        TriggerDetonation(Input.GetKey(GameManager.controls["Detonate"]));

        GameManager.Exit();
    }

    /// <summary>
    /// Accelerates and decelerates the Player using constant rates over time. Also clamps the upper and lower values of the Player's speed.
    /// </summary>
    /// <param name="isAccelerating"></param>
    public void Accelerate(bool isAccelerating)
    {
        Speed += Time.deltaTime * (isAccelerating ? AccelerationRate : DecelerationRate);
        Speed = Mathf.Clamp(Speed, BaseSpeed, MaxSpeed);
    }

    public void TriggerDetonation(bool isDetonating)
    {
        if (!isDetonating)
        {
            return;
        }

        CreateDetonation();
    }

    /// <summary>
    /// Creates a new Detonation, based on the Player's position and cooldown remaining.
    /// </summary>
    public void CreateDetonation()
    {
        if (ActiveDetonations.Count < maximumDetonationCharges)
        {
            GameObject newDetonation = Instantiate(DetonationPrefab);
            newDetonation.transform.position = transform.position;
            ActiveDetonations.Add(newDetonation);
        }
    }

    /// <summary>
    /// Sums together the net force of all gravitational pulls being experienced by the player.
    /// </summary>
    /// <returns></returns>
    public Vector2 CalculateGravity()
    {
        Vector2 gravityTotal = Vector2.zero;

        if (_listOfGravitationalPulls.Count > 0)
        {
            gravityTotal = _listOfGravitationalPulls[0];
            for (int i = 1; i < _listOfGravitationalPulls.Count; i++)
            {
                float newX = 0.5f * gravityTotal.x + _listOfGravitationalPulls[i].x;
                float newY = 0.5f * gravityTotal.y + _listOfGravitationalPulls[i].y;

                gravityTotal = new Vector2(newX, newY);
            }
        }
        _listOfGravitationalPulls.Clear();
        return gravityTotal;
    }

    /// <summary>
    /// Updates the position and lifecycle status of all active Detonations.
    /// </summary>
    void UpdateDetonations()
    {
        foreach (GameObject detonation in ActiveDetonations)
        {
            if (!detonation.GetComponent<Detonation>().Alive)
            {
                DeadDetonations.Add(detonation);
            }

            detonation.transform.position = transform.position;
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

    public void SetDirection(Vector3 newDirection)
    {
        Direction = (Vector2)newDirection;
    }

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection;
    }
}