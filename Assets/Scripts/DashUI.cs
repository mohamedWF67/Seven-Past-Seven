using System;
using TMPro;
using UnityEngine;

public class DashUI : MonoBehaviour
{
    PlayerMovementScript pms;
    [SerializeField] GameObject dashPanel;
    
    private void Start()
    {
        pms = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
    }
    private void Update()
    {
        bool dashActive = pms.dashFinished && pms.canDash;
        dashPanel.SetActive(dashActive);
    }
}
