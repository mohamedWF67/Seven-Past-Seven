using System;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public bool isChecked {set; get;}
    public int checkpointNumber;
    
    private LevelManagerScript lm;

    private void Start()
    {
        lm = FindFirstObjectByType<LevelManagerScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lm.ChangeCheckpoint(checkpointNumber);
        }
    }
}
