using System;
using UnityEngine;

public class OnDeathDrop : MonoBehaviour
{
    [SerializeField] GameObject objectToDrop;
    public int keyIndex;
    private void OnDestroy()
    {
        try
        {
            objectToDrop.TryGetComponent(out DoorKeyScript doorKeyScript);
            doorKeyScript.keyIndex = keyIndex;
        }catch (Exception e)
        {
            Console.WriteLine(e);
        }

        //* Instantiate an object on Gameobject's death.
        if (GetComponent<HealthSystem>().isDead)
            Instantiate(objectToDrop, transform.position, transform.rotation);
    }
}
