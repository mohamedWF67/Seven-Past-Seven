using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    [SerializeField] private float bounceHeight = 20f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
            float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
            float v0 = Mathf.Sqrt(2f * g * bounceHeight) * rb.mass;
            rb.AddForce(Vector2.up *  v0, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(-2.5f,bounceHeight),transform.position + new Vector3(+2.5f,bounceHeight));
    }
}
