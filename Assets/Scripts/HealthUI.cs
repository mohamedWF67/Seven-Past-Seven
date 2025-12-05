using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Tooltip("The text that will be used to display the health.")]
    [SerializeField] TextMeshProUGUI healthText;
    
    [Tooltip("The Image that will be used to display the health.")]
    [SerializeField] GameObject heartMeter;
    [SerializeField] Image heartMeterImage;
    [SerializeField] GameObject shieldImage;
    
    [Tooltip("The hearts objects.")]
    [SerializeField] List<GameObject> hearts;
    
    [Tooltip("The heart levels sprites.")]
    [SerializeField] List<Sprite> heartLevels;
    
    [Tooltip("The Health System.")]
    [SerializeField] HealthSystem hs;
    
    int healthTextTweenId = -1;
    int healthPunchTweenId = -1;
    
    void Awake()
    {
        hs = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        heartMeterImage = heartMeter.GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (hs.isShieled)
        {
            shieldImage.SetActive(true);
            heartMeterImage.color = Color.clear;
        }
        else
        {
            shieldImage.SetActive(false);
            heartMeterImage.color = Color.white;
        }
        if (hs.updateHealthUI)
            UpdateAll();
    }

    private void UpdateAll()
    {
        ChangeHealthLevels(hs.GetHealth());
        ChangeHearts(hs.GetHearts());
        UpdateHealth(hs.GetAbsoluteHealth());
    }

    void ChangeHealthLevels(int health)
    {   //* We divide the health by 10 and -10 to get the levels inversely.
        //* 100/10 = '10' - 10 = 0 || 90/10 = '9' - 10 = -1.
        int heartLevel = Mathf.Abs(health / 10 - 10);
        heartMeterImage.sprite = heartLevels[heartLevel];
    }
    
    void UpdateHealth(int health)
    {   //* Effects for making the animation smoother.
        healthText.gameObject.transform.localScale = Vector3.one;
        if (LeanTween.isTweening(healthPunchTweenId))
            LeanTween.cancel(healthTextTweenId);
        //* Adds a punch effect to the text.
        healthPunchTweenId = LeanTween.scale(healthText.gameObject, Vector3.one * 1.4f, 0.2f)
            .setEasePunch().id;
        //* Cancels the previous animation if it exists.
        if (LeanTween.isTweening(healthTextTweenId))
            LeanTween.cancel(healthTextTweenId);
        //* Animates the health text.
        healthTextTweenId = LeanTween.value(gameObject, int.Parse(healthText.text), health, 0.2f)
            .setOnUpdate((float value) => { healthText.text = value.ToString("000"); }).id;

        healthText.color = hs.healthUIColor;
    }

    void ChangeHearts(int hearts)
    {   //TODO further tests here.
        for (int i = 0; i < this.hearts.Count; i++)
        {
            if (i < hearts - 1)
                this.hearts[i].SetActive(true);
            else
                this.hearts[i].SetActive(false);
        }
    }
}
