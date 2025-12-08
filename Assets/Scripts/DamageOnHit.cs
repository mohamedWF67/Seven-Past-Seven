using System;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    [SerializeField] private float force;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var hitdir = transform.position - other.transform.position;
            hitdir.Normalize();
            
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force / 2, ForceMode2D.Impulse);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-hitdir * force, ForceMode2D.Impulse);
            
            other.gameObject.TryGetComponent(out HealthSystem hs);
            hs.GiveDamage(50);
        }
    }
}
