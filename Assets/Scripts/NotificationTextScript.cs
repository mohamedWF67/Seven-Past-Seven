using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationTextScript : MonoBehaviour
{
    public static NotificationTextScript instance;

    public TextMeshProUGUI notificationText;
    public float timeToShow = 0.5f;
    public float timeVisible = 2f;
    
    Coroutine notificationCoroutine;
    
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        instance = this;
        notificationText.text = "";
    }
    
    public void ShowNotification(String notification)
    {
        notificationText.text = notification;
        if (notificationCoroutine != null) StopCoroutine(notificationCoroutine);
        notificationCoroutine = StartCoroutine(TriggerText(notification));
    }

    IEnumerator TriggerText(String notification)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = notification;
        notificationText.gameObject.transform.localScale = Vector3.one;
        
        LeanTween.scale(notificationText.gameObject, Vector3.one * 1.4f, 0.5f)
            .setEasePunch();
        
        LeanTween.value(notificationText.gameObject, 0, 1, timeToShow).setOnUpdate(UpdateTextAlpha).setEase(LeanTweenType.easeInOutCubic);
        
        yield return new WaitForSeconds(timeVisible + timeToShow);
        
        LeanTween.value(notificationText.gameObject, 1, 0, timeToShow).setOnUpdate(UpdateTextAlpha).setEase(LeanTweenType.easeInOutCubic);
        
        yield return new WaitForSeconds(timeToShow);
        notificationText.gameObject.SetActive(false);
    }
    void UpdateTextAlpha(float alpha)
    {
        Color color = notificationText.color;
        color.a = alpha;
        notificationText.color = color;
    }
}
