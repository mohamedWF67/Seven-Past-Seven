using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHelper : MonoBehaviour
{
    
    [SerializeField] GameObject settingsPanel;
    [SerializeField] SceneAsset sceneToLoad;
    private bool settingsOpen;
    
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene(sceneToLoad.name);
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
