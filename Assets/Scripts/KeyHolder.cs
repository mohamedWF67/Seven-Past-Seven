using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyHolder : MonoBehaviour
{
    public List<int> keyIndexs;
    [SerializeField] bool destroyOnCollect = true;
    [SerializeField] AudioClip keyPickupClip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            int index = other.gameObject.GetComponent<DoorKeyScript>().keyIndex;
            if (CheckIfKeyCollected(index)) return;
            keyIndexs.Add(index);
            Debug.Log($"Key Collected : {index}");
            NotificationTextScript.instance.ShowNotification($"Key Collected : {index}");
            GameManagerScript.instance.AddKey();
            SoundFXManagerScript.instance.PlaySFXSound(keyPickupClip, transform);
            if (destroyOnCollect)
            {
                Destroy(other.gameObject);
                Debug.Log($"Key Destroyed: {index}");
            }
        }
    }
    
    public bool CheckIfKeyCollected(int keyIndex)
    {
        return keyIndexs.Contains(keyIndex);
    }
}
