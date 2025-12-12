using System;
using UnityEngine;

public class OnDeathDrop : MonoBehaviour
{
    [SerializeField] GameObject objectToDrop;
    private void OnDestroy()
    {
        //* Instantiate an object on Gameobject's death.
        if (GetComponent<HealthSystem>().isDead)
            Instantiate(objectToDrop, transform.position, transform.rotation);
    }
}
