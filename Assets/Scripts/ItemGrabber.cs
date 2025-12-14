using System;
using UnityEngine;

public class ItemGrabber : MonoBehaviour
{
    private HealthSystem hs;
    
    void Start()
    {
        hs = GetComponent<HealthSystem>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //* if other is not a collectable skip.
        if (!other.gameObject.CompareTag("Collectables")) return;
        //* Try to get the Item.
        other.TryGetComponent(out Item itemToGrab);
        //* If item not found skip.
        if (itemToGrab == null) return;
        
        Debug.Log($"Item Collected : {itemToGrab.GetItemType()}");
        //* Play the pickup sound.
        if (itemToGrab.pickupSound != null)
            SoundFXManagerScript.instance.PlaySFXSound(itemToGrab.pickupSound, transform);
        //* Switches based on the item's type.
        switch (itemToGrab.GetItemType())
        {
            case 0:
                //* Adds a coin.
                NotificationTextScript.instance.ShowNotification($"You have acquired a coin.");
                GameManagerScript.instance.AddCoin(itemToGrab.value);
                break;
            case 1:
                //* Heals the player.
                NotificationTextScript.instance.ShowNotification($"Health +{itemToGrab.value}");
                hs.Heal(itemToGrab.value);
                break;
            case 2:
                //* Adds an artifact.
                NotificationTextScript.instance.ShowNotification($"You have acquired an artifact.");
                GameManagerScript.instance.AddArtifact();
                break;
        }
        //* Destroys the Item after pickup.
        Destroy(other.gameObject);
    }
}
