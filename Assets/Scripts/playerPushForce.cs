using UnityEngine;

public class playerPushForce : MonoBehaviour
{
    [Tooltip("Control the force of Pushing.")]
    [SerializeField] float pushForce = 20f;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
        }
        
    }
}
