using System;
using UnityEngine;

public class ACT_Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManagerScript.instance.ACT_Finished();
    }
}
