using System;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    private HealthSystem hs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hs = GetComponent<HealthSystem>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Collectables")) return;
        
        if (other.gameObject.GetComponent<Item>() == null) return;
        
        Item item = other.gameObject.GetComponent<Item>();
        Debug.Log($"Item Collected : {item.GetItemType()}");
        switch (item.GetItemType())
        {
            case 0:
                NotificationTextScript.instance.ShowNotification($"Coins : {item.value}");
                GameManagerScript.instance.AddCoin(item.value);
                break;
            case 1:
                NotificationTextScript.instance.ShowNotification($"Health : {item.value}");
                hs.Heal(item.value);
                break;
            case 2:
                NotificationTextScript.instance.ShowNotification($"You have acquired an artifact.");
                GameManagerScript.instance.AddArtifact();
                break;
        }
        Destroy(other.gameObject);
    }
}
