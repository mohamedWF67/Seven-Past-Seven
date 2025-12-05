using System;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ds = GetComponent<DialogueSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        ds.StartDialogue();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        ds.CancelDialogue();
    }
}
