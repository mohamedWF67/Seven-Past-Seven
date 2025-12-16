using System.Collections;
using UnityEditor;
using UnityEngine;

public class FinalFinalSceneScript : MonoBehaviour
{
    public GameObject finalFinalPanel;
    
    public void ShowFinalFinalPanel()
    {
        StartCoroutine(StartFinalFinal());
    }

    IEnumerator StartFinalFinal()
    {
        FullScreenEffectScript.instance.Blink();
        yield return new WaitForSeconds(FullScreenEffectScript.instance.blinkTime);
        finalFinalPanel.SetActive(true);
    }
    
}
