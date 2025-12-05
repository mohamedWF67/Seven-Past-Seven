using UnityEngine;

public class DialogueSystemBox : MonoBehaviour
{
    private DialogueSystem currentDS;
    
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
    
}
