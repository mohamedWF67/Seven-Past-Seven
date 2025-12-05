using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityMenuUI : MonoBehaviour
{
    private float abilityCooldown;
    private float passiveCooldown;
    
    [SerializeField]private GameObject abilityImage;
    [SerializeField]private GameObject passiveImage;

    [SerializeField]private Image abilityIcon;
    [SerializeField]private Image passiveIcon;
    
    [SerializeField]private ShootingScript ss;
    
    private void Start()
    {
        ss = PlayerInput.GetPlayerByIndex(0).gameObject.GetComponentInChildren<ShootingScript>();
    }
    private void Update()
    {
        if (abilityCooldown < 1f)
            abilityIcon.color = new Color(1f,1f,1f,0.2f);
        if (passiveCooldown < 1f)
            passiveIcon.color = new Color(1f,1f,1f,0.2f);
        
        abilityCooldown = ss.abilityCooldown;
        passiveCooldown = ss.passiveCooldown;
        
        abilityImage.GetComponent<Image>().fillAmount = abilityCooldown;
        passiveImage.GetComponent<Image>().fillAmount = passiveCooldown;

        abilityIcon.sprite = ss.GetAbilityImage();
        passiveIcon.sprite = ss.GetPassiveImage();
        
        if (abilityCooldown >= 1f)
            abilityIcon.color = new Color(1f,1f,1f,1f);
        if (passiveCooldown >= 1f)
            passiveIcon.color = new Color(1f,1f,1f,1f);
    }
}
