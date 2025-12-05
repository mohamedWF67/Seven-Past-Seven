using System.Collections;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [Tooltip("Player's layer mask.")]
    [SerializeField] private LayerMask playerMask;
    
    [Tooltip("Maximum distance for detecting player.")]
    [SerializeField] private float maxDistance;
    
    [Tooltip("Damage the player will take.")]
    [SerializeField] private int playerDamage;
    
    [Tooltip("Time till Destroying the trap.")]
    float destroyDelay;
    Rigidbody2D rb;

    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        destroyDelay = Mathf.Sqrt(2f * maxDistance / g);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsPlayerDirectlyUnder())
            StartCoroutine(TrapFall());
        
    }
    
    bool IsPlayerDirectlyUnder()
    {   //* Checks if the player is directly under the trap.
        var hitPlayer = Physics2D.Raycast(transform.position, Vector2.down, maxDistance, playerMask);
        return hitPlayer.collider != null;
    }

    IEnumerator TrapFall()
    {   //* Makes the trap fall.
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   //* Destroys the trap on touching the player or ground.
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<HealthSystem>().GiveDamage(playerDamage);
            Destroy(gameObject);
        }else if (other.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + maxDistance * Vector3.down);
    }
}
