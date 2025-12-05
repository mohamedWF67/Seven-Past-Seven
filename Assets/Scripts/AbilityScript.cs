using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AbilityScript : MonoBehaviour
{
    public AbilityType abilityType;
    
    private LayerMask BlockExplosionLayer;
    
    [SerializeField] public LayerMask HitLayer;
    
    private ParticleSystem ps;
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sr;
    private Light2D _light;
    
    [SerializeField] private Light2D babyLight;
    [SerializeField] private ParticleSystem babyps;
    
    Coroutine abilityCoroutine;
    private void Awake()
    {
        HitLayer = LayerMask.GetMask("Enemy");
        BlockExplosionLayer = LayerMask.GetMask("Ground");
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
        
        if (transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            babyLight = child.GetComponent<Light2D>();
            babyps = child.GetComponent<ParticleSystem>();
        }
        
        abilityCoroutine = StartCoroutine(TriggerAbility());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!abilityType.isSticky) return;
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Enemy")){
            
            gameObject.transform.parent = other.gameObject.transform;
            
            rb.linearVelocity = Vector2.zero;
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
            
        }
    }

    IEnumerator TriggerAbility(bool isInstant = false)
    {
        yield return new WaitForSeconds(0.05f);
        
        if (!isInstant)
            yield return new WaitForSeconds(abilityType.delayTime);
        
        if (ps != null) ps.Play();
        
        if (sr != null) sr.enabled = false;

        rb.linearVelocity = Vector2.zero;
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
        
        if (coll != null) coll.enabled = false;
        
        
        if (_light != null)
        {
            _light.enabled = true;
            LeanTween.value(gameObject,0,abilityType.Intenisty,abilityType.duration / 4).setEase(LeanTweenType.easeOutExpo).setOnUpdate(val=>_light.intensity = val);
            LeanTween.value(gameObject,abilityType.Intenisty,0,abilityType.duration / 4).setEase(LeanTweenType.easeInExpo).setOnUpdate(val=>_light.intensity = val);
        }
        
        if (babyLight != null)
        {
            float intensity = babyLight.intensity;
                                         
            LeanTween.value(gameObject, intensity, 0, abilityType.duration / 4).setOnUpdate
            ( val =>
                {
                    babyLight.intensity = val;
                }
            ).setEase(LeanTweenType.easeOutSine);
        }
        if (babyps != null)
        {
            var emission = babyps.emission;
            emission.enabled = false;
        }

        if (abilityType.delayffect > 0)
            yield return new WaitForSeconds(abilityType.delayffect);
        if (!abilityType.isStaticAbility)
            GetEnemiesInRange();
        else
            StartCoroutine(DamageOverTime());

        yield return new WaitForSeconds(abilityType.duration);
        Destroy(gameObject);
        
        abilityCoroutine = null;

    }

    IEnumerator DamageOverTime()
    {
        while (true)
        {
            GetEnemiesInRange();
            yield return new WaitForSeconds(abilityType.tickRate);
        }
    }
    
    void GetEnemiesInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, abilityType.range, HitLayer);
        
        foreach (Collider2D hit in hits)
        {
            if (hit == null) continue;
            
            HealthSystem enemy = hit.GetComponent<HealthSystem>();
            
            if (enemy == null) continue;

            if (!abilityType.canPathThroughWalls)
            {
                Vector2 direction = hit.transform.position - transform.position;

                RaycastHit2D blockCheck = Physics2D.Raycast(transform.position, direction.normalized,
                    direction.magnitude, BlockExplosionLayer);

                if (blockCheck.collider != null) continue;
            }

            enemy.GiveDamage(abilityType.damage);
            
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
