using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealingField : MonoBehaviour
{
    [Header("Healing")]
    [Tooltip("The time between each healing tick.")]
    [SerializeField] float healTickTime;
    [Tooltip("The amount of health the player will be healed.")]
    [SerializeField] int healAmount;
    
    private HealthSystem hs;
    Coroutine healingCoroutine;
    
    [Header("Animation")]
    [Tooltip("The easing type of the animation.")]
    [SerializeField] private LeanTweenType easeType;
    [SerializeField,Range(0.01f,5f)] private float duration;
    [SerializeField,Range(0.01f,5f)] private float startIntensity;
    [SerializeField,Range(0.01f,5f)] private float endIntensity;
    private Light2D _light;
    int tweenId;
    
    private void Start()
    {
        _light = GetComponent<Light2D>();
        StartTween();
    }

    void StartTween()
    {
        LeanTween.value(gameObject, startIntensity, endIntensity, duration)
            .setEase(easeType)
            .setLoopPingPong(-1)
            .setOnUpdate((float val) =>
            {
                _light.intensity = val;
            });
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        //* Starts the healing effect.
        hs = other.GetComponent<HealthSystem>();
        healingCoroutine = 
            StartCoroutine(hs.ExternalHealing(duration,healAmount));
        
    }
    
    private void OnValidate()
    {
        if (healingCoroutine != null)
        {   
            //* Restarts the healing effect.
            StopCoroutine(healingCoroutine);
            healingCoroutine = null;
            healingCoroutine = 
                StartCoroutine(hs.ExternalHealing(duration,healAmount));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        //* Stops the healing effect.
        if (healingCoroutine != null)
        {
            StopCoroutine(healingCoroutine);
            healingCoroutine = null;
        }
        hs = null;
    }
}
