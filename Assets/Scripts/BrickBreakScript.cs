using System;
using UnityEngine;

public class BrickBreakScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<ColoredFlash>().Flash(color: Color.red);
            Destroy(gameObject,0.1f);
        }
    }
}
