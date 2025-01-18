using UnityEngine;

public class BorderCollision : MonoBehaviour
{
    Vector3 lastPosition;
    Vector2 cameraCentrePosition;
    Vector2 positionVectorFromCameraCentre;
    Renderer componentRenderer;

    //If wrapAround is set to false initially, WrapAround() will be called because the component is NOT visible in the initial update cycle.
    bool wrapAround = true;
    Collider2D Collider;

    float increasingMagnitude = 0;
    int strike = 0;

    private void Start()
    {
        lastPosition = transform.position;
        cameraCentrePosition = (Vector2)Camera.main.transform.position;
        componentRenderer = GetComponent<Renderer>();
        Collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        //If the component is NOT visible, and it hasn't just been wrapped around, wrap around to the opposite side.
        if (!componentRenderer.isVisible && !wrapAround)
        {
            WrapAround();
        }
        else if(componentRenderer.isVisible && wrapAround)
        {
            //Once the component becomes visible again, set wrapAround to false.
            wrapAround = false;
            Collider.enabled = true;
        }
        else if(!componentRenderer.isVisible && wrapAround)
        {
            //If the distance from the centre of the camera NOW is greater than the distance from the centre in the previous frame,
            //and the component is NOT visible, then it is moving away from the viewport, and it needs to be pulled back in.
            float differenceInMagnitude = (-cameraCentrePosition + (Vector2)transform.position).magnitude - (-cameraCentrePosition + (Vector2)lastPosition).magnitude;
            if (differenceInMagnitude > 0)
            {
                increasingMagnitude = increasingMagnitude < differenceInMagnitude ? differenceInMagnitude : 0;
                strike += increasingMagnitude > 0 ? 1 : -strike;

                if(strike > 2)
                {
                    RedirectToCentre();
                    increasingMagnitude = 0;
                }
            }
            else
            {
                increasingMagnitude = 0;
            }
        }
    }

    /// <summary>
    /// Repositions the component on the opposite side of the plane, using the camera's centre as a reference point.
    /// Then set wrapAround to true, so that the next Update() cycle doesn't get stuck in a loop of wrapping the component around
    /// before the component has the chance to become visible again.
    /// To prevent objects that have been wrapped around from colliding with each other and spiralling in other directions before they become
    /// visible again, the component's collider is turned off during this time.
    /// </summary>
    void WrapAround()
    {
        positionVectorFromCameraCentre = -cameraCentrePosition + (Vector2)transform.position;
        transform.position = cameraCentrePosition - positionVectorFromCameraCentre;
        lastPosition = transform.position;
        wrapAround = true;
        Collider.enabled = false;
    }

    /// <summary>
    /// Occasionally, an object might miss all checks and spiral away from the Viewport.
    /// In that case, it becomes necessary to redirect it towards the centre of the viewport to bring it back into view.
    /// </summary>
    void RedirectToCentre()
    {
        Control control;
        Movement movement;
        if (TryGetComponent<Control>(out control))
        {
            control.SetDirection(-(Vector2)transform.position + cameraCentrePosition);
        }
        else if (TryGetComponent<Movement>(out movement))
        {
            movement.SetMovementDirection(-(Vector2)transform.position + cameraCentrePosition);
        }
    }
}
