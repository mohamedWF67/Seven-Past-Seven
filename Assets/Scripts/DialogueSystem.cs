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
    
    public List<string> lines;
    public int lineIndex;
    
    public AudioClip dialogueSound;

    public float speed;
    private bool isDialogueFinished;
    private bool isInDialogue;
    
    [SerializeField] private bool isActMover;
    [SerializeField] private bool isMandatory;
    
    private InputAction skipBtn;
    private InputAction escapeBtn;


    private void Awake()
    {
        skipBtn = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Skip");
        escapeBtn = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Exit");
    }
     
    private void LateUpdate()
    {
        if (skipBtn.triggered && isInDialogue)
            NextLine();
        else if (escapeBtn.triggered && isInDialogue && !isMandatory)
            CancelDialogue();
    }

    public void StartDialogue()
    {
        if (isDialogueFinished) return;
        
        dsBox = Instantiate(dialogueBox).GetComponentInChildren<DialogueSystemBox>();
        isInDialogue = true;
        GameManagerScript.instance.inUI = true;
        dsBox.SetDialogueSystem(this);
        lineIndex = 0;
        StartCoroutine(Type());
    }

    public void NextLine()
    {
        if (dsBox.IsEqualTo(lines[lineIndex]))
        { 
            if (lineIndex < lines.Count - 1)
            {
                lineIndex ++;
                dsBox.ClearText();
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
            dsBox.UpdateText(lines[lineIndex]);
        }
    }

    void onDialogueFinish()
    {
        if (isActMover) ACT_Setter.instance.NextAct();
    }
    
    IEnumerator Type()
    {
        if (dialogueSound != null)
            AudioSource.PlayClipAtPoint(dialogueSound, transform.position);
        
        dsBox.ClearText();
        foreach (char letter in lines[lineIndex])
        {
            dsBox.AddLetter(letter);
            yield return new WaitForSeconds(speed);
        }
        
    }
    
    public void CancelDialogue()
    {
        if (!isInDialogue) return;
        
        isInDialogue = false;
        StopAllCoroutines();
        lineIndex = 0;
        dsBox.ClearText();
        GameManagerScript.instance.inUI = false;
        Destroy(dsBox.gameObject.transform.parent.gameObject);
        dsBox = null;
    }

    public void SkipToEnd()
    {
        while (lineIndex < lines.Count - 1) NextLine();
    }
}
