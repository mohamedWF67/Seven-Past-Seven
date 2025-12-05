using System;
using System.Collections;
using UnityEngine;

public class FireBurnScript : MonoBehaviour
{
    [Tooltip("The time between each damage tick.")]
    [SerializeField,Range(0.01f,5f)] float burnTickTime = 0.5f;
    [Tooltip("The damage the player will take.")]
    [SerializeField,Range(1,100)] int burnDamage = 10;
    
    [SerializeField] bool DecayOnExit;
    [SerializeField] float decayTime;
    Coroutine DecayCoroutine;
    Coroutine burnCoroutine;
    private HealthSystem hs;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        hs = other.GetComponent<HealthSystem>();
        //* Starts the burning effect.
        if (DecayCoroutine != null)
        {
            StopCoroutine(DecayCoroutine);
            DecayCoroutine = null;
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;
        }

        if (burnCoroutine != null) return;
        burnCoroutine = 
            StartCoroutine(hs.ExternalDamageConstant(burnTickTime, burnDamage));

    }

    private void OnValidate()
    {
        if (burnCoroutine != null)
        {   
            //* Restarts the burning effect.
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;
            burnCoroutine =
                StartCoroutine(hs.ExternalDamageConstant(burnTickTime, burnDamage));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        //* Stops the burning effect.
        DecayCoroutine = StartCoroutine(Decay());
           
    }

    IEnumerator Decay()
    {
        if (DecayOnExit)
            yield return new WaitForSeconds(decayTime);
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;   
        }
        hs = null;
    }
}
