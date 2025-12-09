using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    Camera cam;
    private Vector3 mousePos;
    bool isFiring;
    public bool canFire = true;
    private PlayerInput pi;
    private InputAction ia;
    private InputAction lookAction;
    public List<ShotType> shotTypes;
    public List<AbilityType> ULTs;
    private Vector2 aimInput;
    private InputAction switchAbility;
    
    public enum Shooter
    {
        Clouden = 0,
        Blazzy = 1,
        Echo = 2,
        Aira = 3,
        Mallow = 4,
        Racci = 5
    }
    int currentShooterIndex => (int)currentShooter;
    
    public Shooter currentShooter;
    
    Coroutine firingCoroutine;
    Coroutine abilityCoroutine;
    
    
    public float abilityCooldown = 1f;
    public float passiveCooldown = 1f;
    
    private void Awake()
    {
        pi = GetComponentInParent<PlayerInput>();
        cam = Camera.main;
        ia = pi.actions.FindAction("Attack");
        lookAction = pi.actions.FindAction("Look");
        switchAbility = pi.actions.FindAction("SwitchAbility");
    }

    private void Update()
    {
        if (GameManagerScript.instance.inUI) return;
        
        ActionChecks();
    }

    private void ActionChecks()
    {
        if (!canFire) return;
        
        if (switchAbility.triggered && abilityCoroutine == null)
        {
            abilityCoroutine = StartCoroutine(PerformAbility());
        }
        
        if (pi.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 rot = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        
        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            aimInput = lookAction.ReadValue<Vector2>();
            if (aimInput.sqrMagnitude > 0.1f)
            {
                float rotZ = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
            }
        }
        
        if (ia.IsPressed() && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(Fire());
        }
    }

    IEnumerator PerformAbility()
    {
        
        GameObject bullet = Instantiate(ULTs[currentShooterIndex].Effect, transform.position , Quaternion.identity);

        Vector3 dir = transform.right;
        
        var ability = bullet.AddComponent<AbilityScript>();
        ability.abilityType = ULTs[currentShooterIndex];

        if (!ULTs[currentShooterIndex].isStaticAbility)
        {
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x,dir.y).normalized * ULTs[currentShooterIndex].force,ForceMode2D.Impulse);
        }
        else
        {
            bullet.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            bullet.gameObject.transform.parent = gameObject.transform;
        }

        //Debug.Log("Ability Performed");

        if (currentShooter == Shooter.Echo)
        {
            float totalTime = ULTs[currentShooterIndex].duration + ULTs[currentShooterIndex].delayTime + ULTs[currentShooterIndex].delayffect;
            CameraShake.Instance.Shake(5,1,totalTime);
        }
        
        float cooldown = ULTs[currentShooterIndex].cooldown;
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            abilityCooldown = 100 - (int)(cooldown / ULTs[currentShooterIndex].cooldown * 100);
            abilityCooldown = Mathf.Clamp(abilityCooldown, 0, 100);
            abilityCooldown /= 100f;
            
            yield return null;
        }
        //Debug.Log("Can Throw again");
        
        abilityCoroutine = null;
    }

    IEnumerator Fire()
    {
        isFiring = true;
        GameObject bullet = Instantiate(shotTypes[currentShooterIndex].bullet, transform.position , Quaternion.identity);

        Vector3 dir = transform.right;
        
        var behaviour = bullet.AddComponent<ProjectileBehaviour>();
        behaviour.damage = shotTypes[currentShooterIndex].damage;
        behaviour.DestroyTime = shotTypes[currentShooterIndex].destroyTime;
        behaviour.waitTime = shotTypes[currentShooterIndex].waitTime;
        behaviour.canPathTroughWalls = shotTypes[currentShooterIndex].canPathThroughWalls;
        behaviour.canPathTroughEnemies = shotTypes[currentShooterIndex].canPathThroughEnemies;
        
        bullet.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(dir.x,dir.y).normalized * shotTypes[currentShooterIndex].speed;
        
        if (currentShooter == Shooter.Echo)
        {
            CameraShake.Instance.Shake(0.5f,1);
        }
        
        float cooldown = shotTypes[currentShooterIndex].fireRate;
        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            
            passiveCooldown = 100 - (int)(cooldown / shotTypes[currentShooterIndex].fireRate * 100);
            passiveCooldown = Mathf.Clamp(passiveCooldown, 0, 100);
            passiveCooldown /= 100f;
            
            yield return null;
        }
        
        isFiring = false;
        firingCoroutine = null;
    }

    public int GetDamage()
    {
        return shotTypes[currentShooterIndex].damage;
    }
    
    public float GetFireRate()
    {
        return shotTypes[currentShooterIndex].fireRate;
    }
    public float GetDPS()
    {
        return shotTypes[currentShooterIndex].DPS;
    }

    public Sprite GetAbilityImage()
    {
        return ULTs[currentShooterIndex].icon;
    }

    public Sprite GetPassiveImage()
    {
        return shotTypes[currentShooterIndex].icon;
    }
}
