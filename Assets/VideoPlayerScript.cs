using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] GameObject pausePanel;
    
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

    
    public void Skip()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop();
        
        SceneManager.LoadScene(sceneName);
    }
    
    public void TogglePause()
    {
        if (videoPlayer == null) return;

        if (videoPlayer.isPlaying)
        {
            pausePanel.SetActive(true);
            videoPlayer.Pause();
        }else
        {
            pausePanel.SetActive(false);
            videoPlayer.Play();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TogglePause();
        if (Input.GetKeyDown(KeyCode.Return))
            Skip();
        if (videoPlayer.isPlaying && videoPlayer.time >= videoPlayer.length)
        {
            Skip();
        }
    }
    
}
