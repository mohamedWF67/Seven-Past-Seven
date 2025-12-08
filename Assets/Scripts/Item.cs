using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Coin = 0,
        Heal = 1,
        Artifact = 2
    }
    public ItemType itemType;
    public int value = 1;

    public string Type()
    {
        Debug.Log(itemType.ToString());
        return itemType.ToString();
    }

    public int GetItemType()
    {
        return (int)itemType;
    }

}
