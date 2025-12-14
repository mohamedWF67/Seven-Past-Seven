using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyBulletScript : MonoBehaviour
{
    public float DestroyTime = 1f;
    public int damage = 10;
    public float waitTime;
    private ParticleSystem ps;
    private Collider2D coll;
    private Rigidbody2D rb;
    private float effectDelay = 0.5f;
    private Light2D _light;
    
    private void Start()
    {
        //Debug.Log("Projectile Created");
        ps = GetComponent<ParticleSystem>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        _light = GetComponent<Light2D>();
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime(bool destroy = true)
    {
        if (destroy)
            yield return new WaitForSeconds(DestroyTime);
        
        yield return new WaitForSeconds(waitTime);
        
        if (ps != null)
        {
            var emission = ps.emission;
            emission.enabled = false;
            coll.enabled = false;
        }
        
        if (coll != null) coll.enabled = false;
        
        rb.linearVelocity = Vector2.zero;
        
        if (_light != null)
        {
            float intensity = _light.intensity;
            
            LeanTween.value(gameObject, intensity, 0, effectDelay).setOnUpdate
            ( val =>
                {
                    _light.intensity = val;
                }
            ).setEase(LeanTweenType.easeOutSine);
        }
        yield return new WaitForSeconds(effectDelay);
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            StartCoroutine(DestroyAfterTime(false));
        
        if (other.TryGetComponent( out HealthSystem hs) && other.gameObject.CompareTag("Player"))
        {
            hs.GiveDamage(damage);

            StartCoroutine(DestroyAfterTime(false));
        }
    }
}
