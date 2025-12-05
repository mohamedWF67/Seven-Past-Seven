using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D platformCollision;
    [Tooltip("Time to fall through the platform.")]
    [SerializeField] private float fallTime = 0.5f;

    private void Start()
    {   //* Gets the Platform's main collider. 
        platformCollision = GetComponent<Collider2D>();
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        //* Gets the PlayerMovementScript.
        if (other.gameObject.TryGetComponent(out PlayerMovementScript player))
        {   //* Check if the player is pointing down.
            var down = player.gameObject.GetComponent<PlayerInput>().actions.FindAction("FallDown").triggered;
            Collider2D playerCollider = other.gameObject.GetComponent<BoxCollider2D>();
            if (down)
                StartCoroutine(DisableCollision(playerCollider));
        }
    }
    

    IEnumerator DisableCollision(Collider2D playerCollider)
    {   //* Disables collision.
        Physics2D.IgnoreCollision(playerCollider, platformCollision);
        yield return new WaitForSeconds(fallTime);
        //* Re-enables collision.
        Physics2D.IgnoreCollision(playerCollider, platformCollision,false);
    }
}
