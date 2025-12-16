using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class FinalFinalSceneScript : MonoBehaviour
{
    public GameObject finalFinalPanel;
    private bool gameFinished;
    public void ShowFinalFinalPanel()
    {
        StartCoroutine(StartFinalFinal());
    }

    IEnumerator StartFinalFinal()
    {
        FullScreenEffectScript.instance.Blink();
        yield return new WaitForSeconds(FullScreenEffectScript.instance.blinkTime);
        finalFinalPanel.SetActive(true);
        gameFinished = true;
    }

    private void Update()
    {
        if (gameFinished && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}
