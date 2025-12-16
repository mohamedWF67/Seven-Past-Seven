using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHelper : MonoBehaviour
{
    
    [SerializeField] GameObject settingsPanel;
    #if UNITY_EDITOR
    [SerializeField] SceneAsset nextScene;
    #endif
    [SerializeField, HideInInspector]
    private string sceneName;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (nextScene != null)
            sceneName = nextScene.name;
    }
    #endif
    
    private bool settingsOpen;
    
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene(sceneName);
    }

    public void OpenSettings()
    {
        settingsOpen = !settingsOpen;
        settingsPanel.SetActive(settingsOpen);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}
