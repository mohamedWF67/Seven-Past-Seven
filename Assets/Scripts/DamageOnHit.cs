using System;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private int damage = 50;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var hitDir = transform.position - other.transform.position;
            hitDir.Normalize();
            
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force / 2 * Time.deltaTime, ForceMode2D.Impulse);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-hitDir * force * Time.deltaTime, ForceMode2D.Impulse);
            
            other.gameObject.TryGetComponent(out HealthSystem hs);
            hs.GiveDamage(damage);
        }
    }
}
