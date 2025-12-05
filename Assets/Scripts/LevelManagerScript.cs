using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class LevelManagerScript : MonoBehaviour
{
    [SerializeField] CheckpointScript[] checkpoints;
    [SerializeField] int currentCheckpoint;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        checkpoints = Object.FindObjectsByType<CheckpointScript>(sortMode: FindObjectsSortMode.None);
        
        checkpoints = checkpoints.OrderBy(e => e.transform.GetSiblingIndex())
            .ToArray();
        if (checkpoints != null)
        {
            currentCheckpoint = -1;
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i].checkpointNumber = i;
            }
        }
    }

    public void ChangeCheckpoint(int checkpointNumber)
    {
        if (checkpointNumber > currentCheckpoint && checkpointNumber < checkpoints.Length)
        {
            for (int i = 0; i <= checkpointNumber; i++)
            {
                checkpoints[i].isChecked = true;
            }
            currentCheckpoint = checkpointNumber;
        }
    }

    public void Respawn(GameObject obj)
    {
        if (currentCheckpoint == -1)
        {
            obj.transform.position = new Vector3(0, 1, 0);
            return;
        }
        obj.transform.position =
        checkpoints[currentCheckpoint].gameObject.transform.position + Vector3.up * 0.5f;
    }
}
