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
    private AudioSource audioSource;
    
    [SerializeField] private Light2D babyLight;
    [SerializeField] private ParticleSystem babyps;
    
    Coroutine abilityCoroutine;
    private void Awake()
    {
        //* Sets all the required references.
        HitLayer = LayerMask.GetMask("Enemy");
        BlockExplosionLayer = LayerMask.GetMask("Ground");
        ps = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        _light = GetComponent<Light2D>();
        audioSource = GetComponent<AudioSource>();
        
        if (transform.childCount > 0)
        {
            GameObject child = transform.GetChild(0).gameObject;
            babyLight = child.GetComponent<Light2D>();
            babyps = child.GetComponent<ParticleSystem>();
        }
        
        //* Starts the ability coroutine.
        abilityCoroutine = StartCoroutine(TriggerAbility());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //* If the ability is sticky, it will stick to the ground or enemies.
        if(!abilityType.isSticky) return;
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Enemy")){
            //* Sets the projectile's parent to the other object.
            gameObject.transform.parent = other.gameObject.transform;
            //* Stops the projectile's movement.
            rb.linearVelocity = Vector2.zero;
            //* Sets the rigid body to kinematic to prevent it from moving.
            if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    IEnumerator TriggerAbility(bool isInstant = false)
    {
        //* Waits for 0.05 seconds to prevent the ability from running before getting all the references.
        yield return new WaitForSeconds(0.05f);
        
        if (audioSource != null)
        {
            Debug.Log(abilityType.sound);
            audioSource.clip = abilityType.sound;
            audioSource.loop = abilityType.isLoopingAudio;
            
            if(audioSource != null)
                audioSource.Play();
            Debug.Log(audioSource.clip);
        }
        
        //* Waits for the ability's delay time.
        if (!isInstant)
            yield return new WaitForSeconds(abilityType.delayTime);
        //* Plays the ability particle system.
        if (ps != null) ps.Play();
        //* Disable the sprite renderer
        if (sr != null) sr.enabled = false;
        //* Play Baby Sound
        if (abilityType.babySound != null)
            SoundFXManagerScript.instance.Play3DSFXSound(abilityType.babySound, transform);
        //* Stops the projectile's movement.
        rb.linearVelocity = Vector2.zero;
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;
        //* Disable the collider
        if (coll != null) coll.enabled = false;
        
        //* Starts the light effect.
        if (_light != null)
        {
            _light.enabled = true;
            LeanTween.value(gameObject,0,abilityType.Intenisty,abilityType.duration / 4).setEase(LeanTweenType.easeOutExpo).setOnUpdate(val=>_light.intensity = val);
            LeanTween.value(gameObject,abilityType.Intenisty,0,abilityType.duration / 4).setEase(LeanTweenType.easeInExpo).setOnUpdate(val=>_light.intensity = val);
        }
        //* Stops the moving effect particles light.
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
        //* Stops the moving effect particles.
        if (babyps != null)
        {
            var emission = babyps.emission;
            emission.enabled = false;
        }
        //* If enabled waits for the delay effect time that prevents the enemy from getting damaged before the particles reach it.
        if (abilityType.delayffect > 0)
            yield return new WaitForSeconds(abilityType.delayffect);
        //* if the ability is static then it deals damage over time.
        if (abilityType.isStaticAbility)
        {
            StartCoroutine(DamageOverTime()); 
        }else
        {
            //*else then it deals damage instantly.
            GetEnemiesInRange();
        } 

            
        //* Waits for the ability duration.
        yield return new WaitForSeconds(abilityType.duration);
        //* Destroys the ability's game object.
        Destroy(gameObject);
        //* Resets the ability coroutine.
        abilityCoroutine = null;

    }

    IEnumerator DamageOverTime()
    {
        //* Deals damage to enemies in range according to the tick rate.
        while (true)
        {   //* Gets all the enemies in range.
            GetEnemiesInRange();
            //* Waits for the ability tick rate.
            yield return new WaitForSeconds(abilityType.tickRate);
        }
    }
    
    void GetEnemiesInRange()
    {
        //* Gets all the enemies in range.
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, abilityType.range, HitLayer);
        
        foreach (Collider2D hit in hits)
        {
            //* makes sure that it doesn't hit itself.
            if (hit == null) continue;
            //* Grabs the enemy's health system.
            HealthSystem enemy = hit.GetComponent<HealthSystem>();
            //* Checks if the enemy exists.
            if (enemy == null) continue;
            //* If the ability can path through walls, it will not check for blocks.
            if (!abilityType.canPathThroughWalls)
            {
                //* Calculates the direction of the raycast.
                Vector2 direction = hit.transform.position - transform.position;
                //* Checks if there is a block in the way.
                RaycastHit2D blockCheck = Physics2D.Raycast(transform.position, direction.normalized,
                    direction.magnitude, BlockExplosionLayer);
                //* If there is a block in the way, it will not deal damage.
                if (blockCheck.collider != null) continue;
            }
            //* Deals damage to the enemy.
            enemy.GiveDamage(abilityType.damage);
            
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
