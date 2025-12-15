using System;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    [SerializeField] private float force = 100;
    [SerializeField] private int damage = 50;
    [SerializeField] AudioClip hitSound;
    private void OnCollisionEnter2D(Collision2D other)
    {
        //* if the other is not the player skip.
        if (!other.gameObject.CompareTag("Player")) return;
        
        //* Gets the direction of the player relative to the GameObject.
        Vector2 hitDir = transform.position - other.transform.position;
        //* Normalizes the direction to make it be either 1 or 0.
        hitDir.Normalize();
        //* Multiply the direction with force.
        hitDir *= force;
        //* Play the hit sound.
        if (hitSound != null)
            SoundFXManagerScript.instance.PlaySFXSound(hitSound, transform,0.5f);
        //* Apply a vertical force.
        other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * force / 2 * Time.deltaTime, ForceMode2D.Impulse);
        //* Apply an apposing force in the opposite direction.
        other.gameObject.GetComponent<Rigidbody2D>().AddForce(-hitDir , ForceMode2D.Impulse);
        //* Try to get the health System of the player.
        other.gameObject.TryGetComponent(out HealthSystem hs);
        //* Damage the player.
        hs.GiveDamage(damage);
        
    }
}
