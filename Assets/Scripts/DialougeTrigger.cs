using System;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{
    private DialogueSystem ds;
    private SpriteRenderer sr;

    [SerializeField]private bool isAGhost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ds = GetComponent<DialogueSystem>();
        sr = transform.parent.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (isAGhost)
            LeanTween.value(gameObject, 0f, 0.7f, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate( value =>
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, value);
            });
        
        ds.StartDialogue();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (isAGhost)
            LeanTween.value(gameObject, 0.7f, 0, 1f).setEase(LeanTweenType.easeInOutCubic).setOnUpdate( value =>
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, value);
            });
        
        ds.CancelDialogue();
    }
}
