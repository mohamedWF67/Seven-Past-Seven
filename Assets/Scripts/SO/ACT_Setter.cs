using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ACT_Setter : MonoBehaviour
{
    public static ACT_Setter instance;
    public List<ACT_Data> actData;
    public int currentActIndex;

    public GameObject player;
    public ShootingScript ss;
    public PlayerMovementScript pms;
    public HealthSystem hs;

    private void Awake()
    {
        instance = this;
        
        player = GameObject.FindGameObjectWithTag("Player");
        ss = player.GetComponentInChildren<ShootingScript>();
        pms = player.GetComponent<PlayerMovementScript>();
        hs = player.GetComponent<HealthSystem>();

        if (currentActIndex <= 0)
        {
            NextAct();
        }
    }

    public void SetActData()
    {
        ss.currentShooter = (ShootingScript.Shooter)actData[currentActIndex].CurrentAbilityStack;
        ss.canFire = actData[currentActIndex].canFire;
        pms.canDash = actData[currentActIndex].canDash;
        pms.extraAirJumps = actData[currentActIndex].canDoubleJump ? 1 : 0;
        hs.RefreshHealth(actData[currentActIndex].maxHealth,actData[currentActIndex].maxHearts,actData[currentActIndex].enduranceModifier);
        
        NotificationTextScript.instance.ShowNotification($"You have acquired {actData[currentActIndex].CurrentAbilityStack}'s abilities.");
    }
    
    public void NextAct()
    {
        SetActData();
        currentActIndex++;
    }
    
}
