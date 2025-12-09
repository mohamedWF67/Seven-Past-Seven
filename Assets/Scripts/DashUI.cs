using System;
using TMPro;
using UnityEngine;

public class DashUI : MonoBehaviour
{
    PlayerMovementScript pms;
    [SerializeField] GameObject dashPanel;
    
    bool allReferencesSet => pms != null;
    
    private void Awake()
    {
        pms = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
    }

    void CheckReferences()
    {
        if (!allReferencesSet)
            Awake();
    }
    
    private void Update()
    {
        CheckReferences();
        
        bool dashActive = pms.dashFinished && pms.canDash;
        dashPanel.SetActive(dashActive);
    }
}
