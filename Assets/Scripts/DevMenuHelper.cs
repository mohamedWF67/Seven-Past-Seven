using System;
using UnityEngine;

public class DevMenuHelper : MonoBehaviour
{
    
    [SerializeField] private GameObject devPanel;
    
    private GameObject player;
    private HealthSystem hs;
    private ShootingScript ss;

    private float timer;
    private int devModeClickCount;
    [SerializeField,Range(0,10)] private int devModeClickCountTarget = 10;
    private bool isDevModeOn;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hs = player.GetComponent<HealthSystem>();
        ss = player.GetComponentInChildren<ShootingScript>();
    }

    private void Update()
    {
        DevModeMethod();
    }

    public void DamagePlayer(int damage)
    {
        hs.GiveDamage(damage);
    }

    public void HealPlayer(int heal)
    {
        hs.Heal(heal);
    }

    public void ChangeAbility()
    {
        int abilityIndex = (int)ss.currentShooter + 1 % 6;
        ss.currentShooter = (ShootingScript.Shooter)abilityIndex;
    }
    
    void DevModeMethod()
    {
        if (timer > 5f)
        {
            timer = 0;
            devModeClickCount = 0;
        }
        if (devModeClickCount >= 1)
            timer += Time.deltaTime;
        if (devModeClickCount >= devModeClickCountTarget)
        {
            isDevModeOn = !isDevModeOn;
            devModeClickCount = 0;
        }
        devPanel.SetActive(isDevModeOn);
    }

    public void DevMode()
    {
        devModeClickCount++;
    }
}
