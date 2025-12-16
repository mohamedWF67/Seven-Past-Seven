using System.Collections;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Maximum distance for detecting player.")]
    [SerializeField] private float maxDistance;
    
    [Tooltip("Damage the player will take.")]
    [SerializeField] private int playerDamage;
    
    [Space(5)]
    
    [Tooltip("How quickly trap will fall.")]
    [SerializeField,Range(1f,10f)]private float fallingMass = 1f;
    [Tooltip("How quickly trap will fall.")]
    [SerializeField,Range(1f,10f)]private float fallingGravityScale = 1f;
    
    float destroyDelay;
    Rigidbody2D rb;
    LayerMask playerMask;
    
    private void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        destroyDelay = Mathf.Sqrt(2f * maxDistance / g);
        //* Changes the fall speed of the trap by changing the mass and gravity of the rigidbody.
        rb.mass = fallingMass;
        rb.gravityScale = fallingGravityScale;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMask = LayerMask.GetMask("Player");
        //* Changes the fall speed of the trap by changing the mass and gravity of the rigidbody.
        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        destroyDelay = Mathf.Sqrt(2f * maxDistance / g);
        rb.mass = fallingMass;
        rb.gravityScale = fallingGravityScale;
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
        if (other.gameObject.TryGetComponent(out HealthSystem hs) && other.gameObject.CompareTag("Player")) {
            hs.GiveDamage(playerDamage);
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
