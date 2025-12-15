using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] List<TextMeshProUGUI> textList;
    
    [Header("Dash Panel")]
    PlayerMovementScript pms;
    [SerializeField] GameObject dashPanel;

    [Header("Ability Panel")]
    [SerializeField]private GameObject abilityMenu;
    
    [SerializeField]private GameObject abilityImage;
    [SerializeField]private GameObject passiveImage;

    [SerializeField]private Image abilityIcon;
    [SerializeField]private Image passiveIcon;
    
    [SerializeField]private ShootingScript ss;
    
    private float abilityCooldown;
    private float passiveCooldown;
    
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI scoreText;
    
    bool allReferencesSet => ss != null && pms != null;
    
    private void Awake()
    {
        if (ss == null)
            ss = PlayerInput.GetPlayerByIndex(0).gameObject.GetComponentInChildren<ShootingScript>();
        if (pms == null)
            pms = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementScript>();
    }
    private void LateUpdate()
    {
        if (!allReferencesSet)
            Awake();
        
        UpdateItems();
        UpdateDashPanel();
        UpdateAbilityPanel();
        UpdateScore();
    }
    
    private void UpdateItems()
    {
        textList[0].text = GameManagerScript.instance.keyCount.ToString("000");
        textList[1].text = GameManagerScript.instance.coinCount.ToString("000");
        textList[2].text = GameManagerScript.instance.artifactCount.ToString("000");
    }

    private void UpdateDashPanel()
    {
        bool dashActive = pms.dashFinished && pms.canDash;
        dashPanel.SetActive(dashActive);
    }

    private void UpdateAbilityPanel()
    {
        if (ss != null)
        {
            abilityMenu.SetActive(ss.canFire);
        }
        
        if (abilityCooldown < 1f)
            abilityIcon.color = new Color(abilityIcon.color.r,abilityIcon.color.g,abilityIcon.color.b,0.2f);
        if (passiveCooldown < 1f)
            passiveIcon.color = new Color(passiveIcon.color.r,passiveIcon.color.g,passiveIcon.color.b,0.2f);
        
        abilityCooldown = ss.abilityCooldown;
        passiveCooldown = ss.passiveCooldown;
        
        abilityImage.GetComponent<Image>().fillAmount = abilityCooldown;
        passiveImage.GetComponent<Image>().fillAmount = passiveCooldown;

        abilityIcon.sprite = ss.GetAbilityImage();
        passiveIcon.sprite = ss.GetPassiveImage();
        
        if (abilityCooldown >= 1f)
            abilityIcon.color = new Color(abilityIcon.color.r,abilityIcon.color.g,abilityIcon.color.b,1f);
        if (passiveCooldown >= 1f)
            passiveIcon.color = new Color(passiveIcon.color.r,passiveIcon.color.g,passiveIcon.color.b,1f);
    }
    
    private void UpdateScore()
    {
        scoreText.text = "Score: " + GameManagerScript.instance.score.ToString("00000");
    }
}
