using System;
using TMPro;
using UnityEngine;

public class DialogueSystemBox : MonoBehaviour
{
    public static DialogueSystemBox instance;
    
    private DialogueSystem currentDS;
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        dialogueText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetDialogueSystem(DialogueSystem ds)
    {
        currentDS = ds;
    }
    
    public DialogueSystem GetDialogueSystem()
    {
        return currentDS;
    }

    public void ResetDialogueSystem()
    {
        currentDS = null;
    }

    public void NextLine()
    {
        if (currentDS == null) return;
        currentDS.NextLine();
    }

    public void SkipToEnd()
    {
        if (currentDS != null)
        {
            currentDS.SkipToEnd();
        }
    }

    public void UpdateText(String text)
    {
        dialogueText.text = text;
    }
    
    public void AddLetter(char letter)
    {
        dialogueText.text += letter;
    }

    public String GetText()
    {
        return dialogueText.text;
    }

    public bool IsEqualTo(String text)
    {
        return dialogueText.text == text;
    }

    public void ClearText()
    {
        dialogueText.text = "";
    }

}
