using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Entity Type")]
    [Tooltip("The type of entity that this script is attached to.")]
    [SerializeField] private bool isPlayer = false;
    
    [SerializeField] private bool wasDamaged;
    [SerializeField] private bool showHealthBar;
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private GameObject SpawnedHealthBar;
    
    [Header("Max")]
    [Tooltip("How much health the player can have.")] 
    [SerializeField] private int maxHealth = 100;
    
    [Tooltip("How much hearts can the player have.")] 
    [SerializeField] private int maxHearts = 3;
    
    [Header("Current")]
    [Tooltip("The current health of the player.")] 
    [SerializeField] private int currentHealth;
    
    [Tooltip("The current amount of hearts.")] 
    [SerializeField] private int currentHearts;
    
    [Tooltip("If the player is dead.")]
    [SerializeField] public bool isDead;

    [Header("Endurance")] 
    [Tooltip("If the player is amune to damage.")]
    [SerializeField] public bool isShieled;
    
    [Tooltip("The duration of the shield.")]
    [SerializeField] private float shieldDuration;
    
    [Tooltip("The modifier of the damage taken.")]
    [SerializeField,Range(0f,1f)] float enduranceModifier = 0;
    
    [Header("Regen")]
    [Tooltip("If the player can regenerate health.")]
    [SerializeField] private bool canRegen;
    
    [Tooltip("The amount of heal for health regen per second.")]
    [SerializeField] private int healPerSecond = 1;

    [Tooltip("The delay between each heal fo the regen.")]
    [SerializeField] private float healthRegenDelay = 0.2f;
    
    [Header("Flash")]
    [Tooltip(" If the object flash when taking damage.")]
    [SerializeField] private bool canFlash = true;
    
    [Tooltip("The material to flash the player's sprite.")]
    [SerializeField] private Material flashMaterial;
    
    [Tooltip("Duration of the damage effect.")]
    [SerializeField,Range(0f,1f)] private float damageEffectDuration = 0.125f;
    
    [Header("Damage UI")]
    [Tooltip("The text that shows the damage.")]
    [SerializeField] private GameObject floatingText;
    
    [Tooltip("Enables the popup text that shows the damage.")]
    [SerializeField] private bool enableDamageUI = true;
    
    [Tooltip("Enables the popup text that shows the damage.")]
    [SerializeField] private bool enableHealUI = true;
    
    [HideInInspector]public bool updateHealthUI;
    [HideInInspector]public Color healthUIColor = Color.white;
    private bool fullyHealed => currentHealth == maxHealth && currentHearts == maxHearts;
    private float healthRegenTimer;
    private LevelManagerScript LMS;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private Coroutine damageCoroutine;
    private Coroutine shieldCoroutine;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        currentHearts = maxHearts;
        LMS = FindFirstObjectByType<LevelManagerScript>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
        
    }

    private void Update()
    {
        HealthRegen();
    }

    public void StartShieldCoroutine()
    {
        if(shieldCoroutine == null)
        {
            shieldCoroutine = StartCoroutine(StartShield(shieldDuration));
        }
    }

    void ShowFloatingDamageText(float damage = 0)
    {
        if (!enableDamageUI) return;
        var damagetxt = Instantiate(floatingText, transform.position, Quaternion.identity);
        TextMesh dmgTextMesh = damagetxt.GetComponent<TextMesh>();
        dmgTextMesh.text = damage.ToString();
        
        dmgTextMesh.color = isShieled ? Color.cyan : Color.red;
    }
    
    void ShowFloatingHealText(float heal = 0)
    {
        if (!enableHealUI) return;
        var healtxt = Instantiate(floatingText, transform.position, Quaternion.identity);
        TextMesh healTextMesh = healtxt.GetComponent<TextMesh>();
        healTextMesh.text = heal.ToString();
        healTextMesh.color = Color.green;
        
    }

    public void Flash()
    {
        if (!canFlash) return;
        
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        
        damageCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial; //* Swaps to flash material.
        
        yield return new WaitForSeconds(damageEffectDuration); //* Pauses for "duration" seconds.
        
        spriteRenderer.material = originalMaterial; //* Swaps back to original material.
        damageCoroutine = null; //* Sets the damageCoroutine to null.
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.2f);
        LMS.Respawn(gameObject);
        currentHearts = maxHearts;
        currentHealth = maxHealth;
        StartCoroutine(UpdateHealthUI());
        isDead = false;
    }
    
    public void GiveDamage(int damage)
    {
        damage = Mathf.RoundToInt(damage * (1 - enduranceModifier));
        Flash();
        ShowFloatingDamageText(damage);
        
        if (isShieled) return;
        
        float allHealth = maxHealth * (currentHearts - 1) + currentHealth;
        allHealth -= damage;
        //Debug.Log("allHealth: " + allHealth + " damage: " + damage);
        if (damage >= allHealth + damage)
        {
            allHealth = 0;
            Kill();
        }
        
        float health = allHealth % maxHealth;
        float hearts = (allHealth / maxHealth) + 1;
        
        if (health == 0 && allHealth != 0) {
            health = maxHealth;
            hearts--;
        }
        //Debug.Log("health : " + health + " hearts: " + Mathf.FloorToInt(hearts));
        currentHealth = Mathf.RoundToInt(health);
        currentHearts = Mathf.FloorToInt(hearts);

        if (!wasDamaged && !isPlayer && showHealthBar)
        {
            wasDamaged = true;
            SpawnedHealthBar = Instantiate(enemyHealthBar,transform.position + Vector3.up * 1f * transform.localScale.y ,Quaternion.identity,transform);
        }

        if (SpawnedHealthBar != null)
        {
            SpawnedHealthBar.GetComponent<Slider>().value = (float)currentHealth / (float)maxHealth;
        }
        
        StartCoroutine(UpdateHealthUI());
    }

    public void Heal(int heal)
    {
        Heal(heal,false);
    }
    
    public void Heal(int heal,bool internalHealing = false)
    {
        if (fullyHealed) return;
        float allHealth = maxHealth * (currentHearts - 1) + currentHealth;
        allHealth += heal;
        //Debug.Log("allHealth: " + allHealth + " heal: " + heal);
        if (allHealth > maxHealth * maxHearts)
        {
            allHealth = maxHealth * maxHearts;
        }

        if (!internalHealing)
            ShowFloatingHealText(heal);
        
        float health = allHealth % maxHealth;
        float hearts = (allHealth / maxHealth) + 1;
        if (health == 0 && allHealth != 0) {
            health = maxHealth;
            hearts--;
        }
        //Debug.Log("health : " + health + " hearts: " + Mathf.FloorToInt(hearts));
        
        currentHealth = Mathf.RoundToInt(health);
        currentHearts = Mathf.FloorToInt(hearts);
        
        StartCoroutine(UpdateHealthUI());
    }

    public void Kill()
    {
        //* Kills the player.
        currentHearts = 0;
        currentHealth = 0;
        isDead = true;
        Debug.Log("Die");
        if (isPlayer)
            StartCoroutine(RespawnPlayer());
        else
            Destroy(gameObject);
    }

    public int GetHearts()
    {
        return currentHearts;
    }

    public int GetHealth()
    {
        //* Returns the current health as a percentage.
        return Mathf.RoundToInt(currentHealth / (float)maxHealth * 100);
    }

    public int GetAbsoluteHealth()
    {
        //* Returns the current health.
        return currentHealth;
    }
    
    public float GetEndurance()
    {
        //* Returns the current Endurance.
        return enduranceModifier;
    }

    IEnumerator UpdateHealthUI()
    {
        updateHealthUI = true;
        yield return new WaitForEndOfFrame();
        updateHealthUI = false;
    }

    void HealthRegen()
    {
        if (canRegen && Time.time - healthRegenTimer >= healthRegenDelay && !fullyHealed)
        {
            healthRegenTimer = Time.time;
            Heal(healPerSecond,true);
        }
    }

    public IEnumerator ExternalHealing(float time,int amount = 1)
    {
        while (true)
        {
            Heal(amount);
            yield return new WaitForSeconds(time);
        }
    }
    
    public IEnumerator ExternalDamageConstant(float time,int amount = 1)
    {
        while (true)
        {
            GiveDamage(amount);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator StartShield(float duration)
    {
        isShieled = true;
        Debug.Log("Shielding for " + duration + " seconds.");
        yield return new WaitForSeconds(duration);
        Debug.Log("Shielding over.");
        isShieled = false;
        shieldCoroutine = null;
    }

    public void RefreshHealth( int maxHealth, int maxHearts, float enduranceModifier)
    {
        this.maxHealth = maxHealth;
        this.maxHearts = maxHearts;
        currentHealth = maxHealth;
        currentHearts = maxHearts;
        this.enduranceModifier = enduranceModifier;
        StartCoroutine(UpdateHealthUI());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!isPlayer) return;

        if (other.gameObject.CompareTag("Hazards"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * 50, ForceMode2D.Impulse);
            Flash();
            Kill();
        }
    }
}
