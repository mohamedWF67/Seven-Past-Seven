using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Tooltip("How long till the platform falls.")]
    [SerializeField] float fallDelay = 2f;
    
    [Tooltip("How long till the platform gets destroyed.")]
    [SerializeField] float destroyDelay = 2f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //* fall only if the player is on the platform.
        if (other.gameObject.CompareTag("Player"))
            StartCoroutine(Fall());
        
    }

    IEnumerator Fall()
    {   //* Waits then falls.
        yield return new WaitForSeconds(fallDelay);
        //* Turns the platform into a dynamic object.
        rb.bodyType = RigidbodyType2D.Dynamic;
        //* Destroys the platform after a destroyDelay.
        Destroy(gameObject,destroyDelay);
    }
}
