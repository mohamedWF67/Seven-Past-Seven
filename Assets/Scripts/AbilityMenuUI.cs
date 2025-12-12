using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityMenuUI : MonoBehaviour
{
    [SerializeField]private GameObject abilityMenu;
    
    [SerializeField]private GameObject abilityImage;
    [SerializeField]private GameObject passiveImage;

    [SerializeField]private Image abilityIcon;
    [SerializeField]private Image passiveIcon;
    
    [SerializeField]private ShootingScript ss;
    
    private float abilityCooldown;
    private float passiveCooldown;
    
    bool allReferencesSet => ss != null;
    
    private void Awake()
    {
        CheckReferences();
    }
    
    void CheckReferences()
    {
        if (allReferencesSet) return;
        //Debug.Log($" {gameObject.name} Setting references...");
        if (ss == null)
            ss = PlayerInput.GetPlayerByIndex(0).gameObject.GetComponentInChildren<ShootingScript>();
    }
    
    private void Update()
    {
        CheckReferences();
        
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
}
