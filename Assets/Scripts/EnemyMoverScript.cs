using System;
using UnityEngine;

public class EnemyMoverScript : MonoBehaviour
{
    
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    
    Rigidbody2D rb;
    SpriteRenderer sr;
    
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 nextPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        CalculatePositions();
    }
    
    private void CalculatePositions()
    { 
        Vector3 platformPosition = transform.position;
        //* Calculates the start and end points.
        startPosition = platformPosition +
                        distance * Vector3.left;
        endPosition = platformPosition +
                      distance * Vector3.right;
        nextPosition = endPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //* Moves the platform using MoveTowards.
        rb.transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        if (transform.position == nextPosition)
        {   //* Switches the next position.
            nextPosition = nextPosition == startPosition ? endPosition : startPosition;
        }

        if (nextPosition == startPosition)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    private void OnValidate()
    {
        CalculatePositions();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPosition, endPosition);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
