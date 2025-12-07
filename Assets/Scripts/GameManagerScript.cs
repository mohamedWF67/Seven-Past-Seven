using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    
    [SerializeField] GameObject pauseMenu;
    private InputAction pauseAction;
    private bool gamePaused;
    
    [SerializeField] GameObject actFinishedUI;
    
    [SerializeField] private List<SceneAsset> scenes;
    [SerializeField] private int currentSceneIndex;
    Coroutine loadNextActCoroutine;
    
    public bool inUI = false;
    
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        pauseAction = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Exit");
    }

    private void Update()
    {
        if (pauseAction.triggered) PauseGame();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game...");
    }

    public void PauseGame()
    {
        if (inUI) return;
        
        if (!gamePaused)
        {
            Time.timeScale = 0;
            gamePaused = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            gamePaused = false;
            pauseMenu.SetActive(false);
        }
    }

    public void ACT_Finished()
    {
        Time.timeScale = 0;
        actFinishedUI.SetActive(true);
    }

    public void GoToNextAct()
    {
        Time.timeScale = 1;
        if (loadNextActCoroutine != null) return;
            loadNextActCoroutine = StartCoroutine(DelayedLoad());
    }

    IEnumerator DelayedLoad()
    {
        FullScreenEffectScript.instance.Blink();
        yield return new WaitForSeconds(FullScreenEffectScript.instance.blinkTime);
        actFinishedUI.SetActive(false);
        SceneManager.LoadScene(scenes[currentSceneIndex + 1].name);
        currentSceneIndex++;
        
    }
}
