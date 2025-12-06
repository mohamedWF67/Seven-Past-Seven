using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool isInDialogue;
    
    private InputAction skipBtn;
    private InputAction escapeBtn;


    private void Start()
    {
        dsBox = dialogueBox.GetComponent<DialogueSystemBox>();
        skipBtn = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Skip");
        escapeBtn = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Exit");
    }
     
    private void Update()
    {
        if (skipBtn.triggered && isInDialogue)
            NextLine();
        else if (escapeBtn.triggered && isInDialogue)
            CancelDialogue();
    }

    public void StartDialogue()
    {
        if (isDialogueFinished) return;
        isInDialogue = true;
        GameManagerScript.instance.inUI = true;
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
                CancelDialogue();
                isDialogueFinished = true;
                onDialogueFinish();
            }
        }else
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
            AudioSource.PlayClipAtPoint(dialogueSound, transform.position);
        
        dialogueText.text = "";
        foreach (char letter in lines[lineIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
        
    }
    
    public void CancelDialogue()
    {
        isInDialogue = false;
        GameManagerScript.instance.inUI = false;
        StopAllCoroutines();
        dsBox.ResetDialogueSystem();
        lineIndex = 0;
        dialogueText.text = "";
        dialogueBox.SetActive(false);
    }
}
