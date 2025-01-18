using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShrinkingMass : MonoBehaviour
{
    [SerializeField] Vector3 actualScale;
    [SerializeField] float minScale = 0f;

    float growthRate = 0.1f;
    float shrinkRate = 0.2f;

    enum GrowthState { shrinking, growing, stable};
    GrowthState _growthState;

    // Start is called before the first frame update
    void Start()
    {
        actualScale = gameObject.transform.localScale;
        _growthState = GrowthState.stable;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch((_growthState))
        {
            case GrowthState.shrinking:
                Shrink();
                break;
            case GrowthState.growing:
                Grow();
                break;
            case GrowthState.stable:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Detonation"))
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            _growthState = GrowthState.shrinking;
        }
    }

    private void Shrink()
    {
        if(gameObject.transform.localScale.x >= minScale)
        {
            gameObject.transform.localScale -= (actualScale * shrinkRate * Time.deltaTime);
        }
        else
        {
            gameObject.transform.localScale = Vector3.zero;
            GetComponent<SpriteRenderer>().color = Color.green;
            _growthState = GrowthState.growing;
        }
    }

    private void Grow()
    {
        if (gameObject.transform.localScale.magnitude <= actualScale.magnitude)
        {
            gameObject.transform.localScale += (actualScale * growthRate * Time.deltaTime);
        }
        else
        {
            gameObject.transform.localScale = actualScale;
            GetComponent<SpriteRenderer>().color = Color.magenta;
            _growthState = GrowthState.stable;
        }
    }
}
