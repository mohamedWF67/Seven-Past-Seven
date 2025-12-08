using System;
using UnityEngine;

public class OnDeathDrop : MonoBehaviour
{
    [SerializeField] GameObject objectToDrop;
    private void OnDestroy()
    {
        if (GetComponent<HealthSystem>().isDead)
            Instantiate(objectToDrop, transform.position, transform.rotation);
    }
}
