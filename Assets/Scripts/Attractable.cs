using System;
using UnityEngine;

public class Attractable : MonoBehaviour
{
    
    [Tooltip("The speed at which the object is attracted to the player.")]
    [SerializeField] private float attractSpeed = 1f;
    
    [Tooltip("The radius of the attraction area.")]
    [SerializeField] private float attractionRadius = 1.5f;
    [Space(10)]
    [Tooltip("The distance between the item and the ground (0 = no check).")]
    [SerializeField] private float groundCheckDistance = 1f;
    
    [Tooltip("How close can the object be to the player.")]
    [SerializeField] private float distanceToPlayer = 1;
    //! Not YET IMPLEMENTED.
    [Tooltip("If the Attraction Speed is = to the player's speed.")] 
    [SerializeField] private bool isMemicPlayer;
    
    [SerializeField] private bool isConstantlyAttracting;

    private Vector2 dir;
    private LayerMask groundLayer;
    private LayerMask playerLayer;
    
    [SerializeField] Transform target;

    private void Awake()
    {
        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var isPlayerInRadius = Physics2D.OverlapCircle(transform.position, attractionRadius, playerLayer);

        if (isPlayerInRadius != null && target == null)
        {
            target = isPlayerInRadius.gameObject.transform;
            Debug.Log($"Target: {target.name}");
        }
        
        if (isPlayerInRadius || isConstantlyAttracting && target != null)
        {
            dir = target.position - transform.position;
            var distance = MathF.Abs(dir.magnitude);
            dir.Normalize();
            if (!IsGroundbetweenPlayerAndObject() && distance >= distanceToPlayer)
                transform.Translate(dir * Time.deltaTime * attractSpeed);
        }
    }
    
    bool IsGroundbetweenPlayerAndObject()
    {   //* Checks if the player is directly under the trap.
        var hitPlayer = Physics2D.Raycast(transform.position, dir, groundCheckDistance, groundLayer);
        return hitPlayer.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
