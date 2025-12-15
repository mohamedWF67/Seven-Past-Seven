using System;
using UnityEngine;

public class ACT_Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        GameManagerScript.instance.ACT_Finished();
    }
}
