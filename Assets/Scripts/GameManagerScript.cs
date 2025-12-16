using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    
    #region PAUSE SETTINGS
    [Header("Pause")]
    [SerializeField] GameObject pauseMenu;
    private InputAction pauseAction;
    private bool isGamePaused;
    #endregion
    
    #region ACT SETTINGS
    [Header("Act")]
    [SerializeField] GameObject actFinishedUI;
    public int currentSceneIndex;
    [SerializeField] private List<SceneAsset> scenes;
    Coroutine loadNextActCoroutine;
    #endregion
    
    #region SCORE AREA
    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private float initialScore;
    [SerializeField] float score;
    [SerializeField] private float scaleFactor = 0.5f;
    [SerializeField] private float timeFactor = 60f;
    [SerializeField] private float currentTime;
    [Space(5)]
    [SerializeField] private float scoreMultiplier = 100;
    [SerializeField] private float extraScore;
    [SerializeField] private float extraScaleFactor = 0.5f;
    [SerializeField] private float extraTimeFactor = 120f;
    [Space(5)]
    public float totalScore;
    #endregion
    
    #region ITEMS
    
    public int keyCount;
    public int artifactCount;
    public int coinCount;
    
    #endregion
    
    #region CONTROLS

    private string currentControlScheme;

    #endregion

    #region FINAL SCENE CONTROLS

    [Header("Final Scene")] public bool isFinalSceneRuning;
    #endregion
    
    [HideInInspector] public bool inUI;
    
    private void Awake()
    {
        //* Creates a singleton.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        //* Gets the Pause action from the Player Input.
        pauseAction = PlayerInput.GetPlayerByIndex(0).actions.FindAction("Exit");

        //* Gets the number out of the scene name.
        try
        {
            currentSceneIndex = int.Parse(new string(SceneManager.GetActiveScene().name.Where(char.IsDigit).ToArray())) - 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        //* Sets the current control scheme to the player control scheme.
        currentControlScheme = PlayerInput.GetPlayerByIndex(0).currentControlScheme;

        //* Subscribe to the event that triggers when control scheme changes
        PlayerInput.GetPlayerByIndex(0).onControlsChanged += OnControlsChanged;
    }

    private void Update()
    {
        if (isFinalSceneRuning) return;
        //* pauses the game on pressing the pause button.
        if (pauseAction.triggered) PauseGame();
        //* Updates the score.
        ScoreUpdate();
    }

    public void QuitGame()
    {
        //* Quits the game.
        Application.Quit();
        Debug.Log("Quitting game...");
    }

    public void PauseGame()
    {
        //* The game doesn't pause if there is already a UI opened.
        if (inUI) return;
        
        if (!isGamePaused)
        {
            //* Stops time and sets the pause menu ui Active.
            Time.timeScale = 0;
            isGamePaused = true;
            pauseMenu.SetActive(true);
        }
        else
        {
            //* Re-enable time and disable the pause menu. 
            Time.timeScale = 1;
            isGamePaused = false;
            pauseMenu.SetActive(false);
        }
    }

    public void ACT_Finished()
    {
        //* Pauses time and sets the act finish ui on.
        Time.timeScale = 0;
        inUI = true;
        actFinishedUI.SetActive(true);
    }

    public void GoToNextAct()
    {
        //* Re-enable time and load next scene.
        Time.timeScale = 1;
        inUI = false;
        if (loadNextActCoroutine != null) return;
            loadNextActCoroutine = StartCoroutine(DelayedLoad());
    }

    IEnumerator DelayedLoad()
    {
        //* Trigger a blink effect.
        FullScreenEffectScript.instance.Blink();
        yield return new WaitForSeconds(FullScreenEffectScript.instance.blinkTime);
        //* Disable the act finish ui.
        actFinishedUI.SetActive(false);
        //* Load next scene.
        SceneManager.LoadScene(scenes[currentSceneIndex + 1].name);
        currentSceneIndex++;
        
        loadNextActCoroutine = null;
    }

    #region ITEMS FUNCTIONS

    public void AddKey()
    {
        //* Increments the number of keys in inventory.
        keyCount++;
    }
    
    public void AddArtifact()
    {
        //* Makes sure that the amount of artifacts cannot pass the act index.
        if (artifactCount <= ACT_Setter.instance.currentActIndex + 1)
        {
            //* Increments the number of artifacts in inventory.
            artifactCount++;
            //* Adds a double scored point.
            AddScoreFromPoints(2);
        }else Debug.Log("You already have the maximum amount of artifacts.");
    }

    public void AddCoin(int weight = 1)
    {
        //* Increments the number of coins in inventory.
        coinCount++;
        //* Adds a score based on the coin's weight.
        AddScoreFromPoints(weight);
    }
    
    #endregion

    #region SCORE FUNCTIONS

    public void ScoreUpdate()
    {
        //* Gets the current time.
        float t = Time.time - currentTime;
        //* Sets the score to the new score based on a few parameters.
        score = initialScore * MathF.Pow(scaleFactor,t/timeFactor);
        //* Updates the score's UI based on a 4-digit format.
        scoreText.text = score.ToString("0000");
        //* Adds the score and extra score to the total score.
        totalScore = score + extraScore;
    }

    public void AddScoreFromPoints(float points,float multiplier = 1)
    {
        //* Gets the current time.
        float t = Time.time - currentTime;
        //* Adds a score based on the points given and the multiplier as well as the set parameters.
        float tempScore = points * scoreMultiplier * multiplier * MathF.Pow(extraScaleFactor,t/extraTimeFactor);
        //* Adds the said score to the extra score.
        extraScore += tempScore;
        Debug.Log($"Extra Score: {extraScore} and Temp Score: {tempScore}");
    }

    public void LowerScore(int amount = 100)
    {
        if (initialScore - amount < 2000) return;
        initialScore -= amount;
    }
    #endregion
    
    private void OnControlsChanged(PlayerInput obj)
    {
        //* Gets the current control scheme.
        currentControlScheme = obj.currentControlScheme;
        Debug.Log("Current Input Scheme: " + currentControlScheme);
    }
}
