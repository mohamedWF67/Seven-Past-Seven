using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Custom Path
    [Header("Custom Path")]
    [Tooltip("Use a custom path.")]
    [SerializeField] bool customPath = false;
    
    [Tooltip("The path to follow.")]
    [SerializeField] Transform[] paths;
    #endregion

    #region Position Based Path
    [Header("Position Based Path")]
    [Tooltip("The direction of the platform (0: Vertical, 1: Horizontal).")]
    [SerializeField] bool moveDirection = false;
    
    [Tooltip("The Start point distance from initial point.")]
    [SerializeField] float startDistance;
    Vector3 startPoint;
    
    [Tooltip("The end point distance from initial point.")]
    [SerializeField] float endDistance;
    Vector3 endPoint;
    #endregion
    
    [Header("Settings")]
    [Tooltip("The speed of the platform.")]
    [Range(0.01f,10f),SerializeField] float speed;
    
    [Tooltip("Start moving from the start point (0: Home, 1: Start Point).")]
    [SerializeField] bool moveFromStart;
    
    private Rigidbody2D rb;
    private Vector3 platformPosition;
    private Vector3 nextPosition;

    private void OnDrawGizmosSelected()
    {   
        platformPosition = transform.position;
        //* Draws the platform's End and Start point illustrations.
        Gizmos.color = Color.red;
        Gizmos.DrawCube(startPoint, transform.localScale);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(endPoint, transform.localScale);
    }

    private void OnValidate()
    {
        CalculatePositions();
        if (moveFromStart)
            transform.position = startPoint;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        platformPosition = rb.transform.position;
        CalculatePositions();
        nextPosition = endPoint;
        if (moveFromStart)
            transform.position = startPoint;
        
    }

    private void CalculatePositions()
    {
        if (customPath && paths.Length > 0)
        {   //* Sets the start and end points.
            startPoint = paths[0].position;
            endPoint = paths[1].position;
        }
        else
        {   //* Calculates the start and end points.
            startPoint = platformPosition + 
                         startDistance * (moveDirection ? Vector3.left : Vector3.down);
            endPoint = platformPosition +
                       endDistance * (moveDirection ? Vector3.right : Vector3.up);
        }
    }

    void Update()
    {   //* Moves the platform using MoveTowards.
        rb.transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);
        if (transform.position == nextPosition)
        {   //* Switches the next position.
            nextPosition = (nextPosition == startPoint) ? endPoint : startPoint;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {   //* Moves the player with the platform.
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = transform;
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {   //* Unsticks the player from the platform.
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;
        }
    }

    private void OnDrawGizmos()
    {   //* Draws the platform's path.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint, platformPosition);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(platformPosition, endPoint);
    }
}
