using System;
using System.Collections;
using UnityEngine;

public class DecayingDamageEnemyScript : MonoBehaviour
{
    public int damagePerTick = 15;
    public float tickRate = 0.5f;
    
    public float Range = 4f;
    public bool playerInRange;
    [SerializeField] Coroutine damageCoroutine;

    private HealthSystem playerHS;
    
    void Start()
    {
        playerHS = FindAnyObjectByType<PlayerMovementScript>().GetComponent<HealthSystem>();
    }
    
    void Update()
    {
            playerInRange = Physics2D.OverlapCircle(transform.position, Range, LayerMask.GetMask("Player"));
            if (playerInRange && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayer());
            }
    }

    IEnumerator DamagePlayer()
    {
        playerHS.GiveDamage(damagePerTick);
        yield return new WaitForSeconds(tickRate);
        damageCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
