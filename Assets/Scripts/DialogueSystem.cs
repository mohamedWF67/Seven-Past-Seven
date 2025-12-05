using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox;
    public DialogueSystemBox dsBox;
    public TextMeshProUGUI dialogueText;
    public List<string> lines;
    public int lineIndex;
    
    public AudioClip dialogueSound;

    public float speed;
    private bool isDialogueFinished;


    private void Start()
    {
        dsBox = dialogueBox.GetComponent<DialogueSystemBox>();
    }

    public void StartDialogue()
    {
        if (isDialogueFinished) return;
        dsBox.SetDialogueSystem(this);
        dialogueBox.SetActive(true);
        lineIndex = 0;
        StartCoroutine(Type());
    }

    public void NextLine()
    {
        if (dialogueText.text == lines[lineIndex])
        { 
            
            if (lineIndex < lines.Count - 1)
            {
            
                lineIndex ++;
                dialogueText.text = "";
                StartCoroutine(Type());
            }
            else
            {
                dialogueBox.SetActive(false);
                isDialogueFinished = true;
                onDialogueFinish();
            }
            
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = lines[lineIndex];
        }
    }

    void onDialogueFinish()
    {
        //* Do something here.
    }
    
    IEnumerator Type()
    {
        if (dialogueSound != null)
        {
            AudioSource.PlayClipAtPoint(dialogueSound, transform.position);
        }
        
        dialogueText.text = "";
        foreach (char letter in lines[lineIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
        
    }
    
    public void CancelDialogue()
    {
        StopAllCoroutines();
        dsBox.ResetDialogueSystem();
        lineIndex = 0;
        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }
}
