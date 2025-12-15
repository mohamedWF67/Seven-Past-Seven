using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ACT_Setter : MonoBehaviour
{
    public static ACT_Setter instance;
    public List<ACT_Data> actData;
    public int currentActIndex = -1;
    
    [Header("Sounds")]
    [SerializeField] AudioClip abilityGainedSound;
    
    GameObject player;
    ShootingScript ss;
    PlayerMovementScript pms;
    HealthSystem hs;
    
    bool allReferencesSet => player != null && ss != null && pms != null && hs != null;

    private void Awake()
    {
        if (instance != null && instance != this) return;
        
        instance = this;
        
        player = GameObject.FindGameObjectWithTag("Player");
        ss = player.GetComponentInChildren<ShootingScript>();
        pms = player.GetComponent<PlayerMovementScript>();
        hs = player.GetComponent<HealthSystem>();
        
        currentActIndex = int.Parse(new string(SceneManager.GetActiveScene().name.Where(char.IsDigit).ToArray())) - 2;
        
        if (currentActIndex <= -1)
        {
            NextAct();
        }
    }

    private void Update()
    {
        if (!allReferencesSet)
        {
            Awake();
        }
    }

    public void SetActData()
    {
        ss.ChangeShooter(actData[currentActIndex].CurrentAbilityStack);
        ss.canFire = actData[currentActIndex].canFire;
        pms.canDash = actData[currentActIndex].canDash;
        pms.extraAirJumps = actData[currentActIndex].canDoubleJump ? 1 : 0;
        
        hs.RefreshHealth(actData[currentActIndex].maxHealth,actData[currentActIndex].maxHearts,actData[currentActIndex].enduranceModifier);

        if (currentActIndex == 0) return;
        NotificationTextScript.instance.ShowNotification($"You have acquired {actData[currentActIndex].CurrentAbilityStack}'s abilities.");
        if(abilityGainedSound != null)
            SoundFXManagerScript.instance.PlaySFXSound(abilityGainedSound, transform);
    }

    public void NextAct()
    {
        currentActIndex++;
        SetActData();
    }
    
}
