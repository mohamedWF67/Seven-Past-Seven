using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    
    [Header("Pause")]
    [SerializeField] GameObject pauseMenu;
    private InputAction pauseAction;
    private bool gamePaused;
    
    [Header("Act")]
    [SerializeField] GameObject actFinishedUI;
    
    [SerializeField] private int currentSceneIndex;
    [SerializeField] private List<SceneAsset> scenes;
    Coroutine loadNextActCoroutine;

    #region SCORE AREA
    [Header("Score")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private float initialScore;
    public float score;
    [SerializeField] private float scaleFactor = 0.5f;
    [SerializeField] private float timeFactor = 60f;
    [SerializeField] private float currentTime;
    [Space(5)]
    [SerializeField] private float scoreMultiplier = 100;
    [SerializeField] private float extraScore;
    [SerializeField] private float extraScaleFactor = 0.5f;
    [SerializeField] private float extraTimeFactor = 120f;
    [Space(5)]
    [SerializeField] private float totalScore;
    #endregion
    
    [HideInInspector] public bool inUI = false;
    
    #region ITEMS
    
    [SerializeField]private int artifactCount;
    [SerializeField]private int coinCount;
    
    #endregion
    
    
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
        
        ScoreUpdate();
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
        inUI = true;
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

    #region ITEMS FUNCTIONS

    public void AddArtifact()
    {
        if (artifactCount <= ACT_Setter.instance.currentActIndex + 1)
        {
            artifactCount++;
        }
    }

    public void AddCoin()
    {
        coinCount++;
    }
    
    #endregion

    #region SCORE FUNCTIONS

    public void ScoreUpdate()
    {
        float t = Time.time - currentTime;
        score = initialScore * MathF.Pow(scaleFactor,t/timeFactor);
        scoreText.text = score.ToString("0000");
        totalScore = score + extraScore;
    }

    public void AddScoreFromPoints(float points,float multiplier = 1)
    {
        float t = Time.time - currentTime;
        float tempScore = points * scoreMultiplier * multiplier * MathF.Pow(extraScaleFactor,t/extraTimeFactor);
        extraScore += tempScore;
        Debug.Log($"Extra Score: {extraScore} and Temp Score: {tempScore}");
    }  
    #endregion
}
