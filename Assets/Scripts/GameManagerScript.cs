using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    [SerializeField] GameObject pauseMenu;
    public bool inUI = false;
    
    private bool gamePaused = false;
    
    private InputAction pauseAction;
    
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
}
