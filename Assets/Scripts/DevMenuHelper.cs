using System;
using UnityEngine;

public class DevMenuHelper : MonoBehaviour
{
    
    [SerializeField] private GameObject devPanel;
    
    private GameObject player;
    private HealthSystem hs;
    private ShootingScript ss;
    private Vector3 exitPosition;

    private float timer;
    private int devModeClickCount;
    [SerializeField,Range(0,10)] private int devModeClickCountTarget = 10;
    private bool isDevModeOn;
    
    int currentCheckpointIndex;
    
    bool allReferencesSet => player != null && hs != null && ss != null;
    
    private void Awake()
    {
        CheckReferences();
    }

    private void Update()
    {
        CheckReferences();
        
        DevModeMethod();
    }
    
    void CheckReferences()
    {
        if (allReferencesSet) return;
        
        //Debug.Log($"{gameObject.name}  Setting references...");
        
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (hs == null)
            hs = player.GetComponent<HealthSystem>();
        if (ss == null)
            ss = player.GetComponentInChildren<ShootingScript>();
        if (exitPosition == Vector3.zero)
        {
            try
            {
                exitPosition = GameObject.FindGameObjectWithTag("Exit").transform.position;
            }
            catch
            {
                Debug.LogError("Exit not found!");
            }
        }
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
        int abilityIndex = ((int)ss.currentShooter + 1) % ss.shotTypes.Count;
        Debug.Log($"Ability index: {(ShootingScript.Shooter)abilityIndex}");
        ss.currentShooter = (ShootingScript.Shooter)abilityIndex;
    }

    public void TeleportPlayerToExit()
    {
        player.transform.position = GameObject.FindGameObjectWithTag("Exit").transform.position + Vector3.up * 1;
    }

    public void TeleportPlayerThroughCheckpoints()
    {
        int checkpointCount = LevelManagerScript.instance.checkpoints.Length;
        
        int index = currentCheckpointIndex % checkpointCount;
        currentCheckpointIndex++;
        
        player.transform.position = LevelManagerScript.instance.checkpoints[index].transform.position + Vector3.up * 1f;
        
        Debug.Log($"Teleported to checkpoint {index + 1}");
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
