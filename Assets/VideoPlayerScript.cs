using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerScript : MonoBehaviour
{

    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] GameObject pausePanel;
    [SerializeField] SceneAsset nextScene;

    public void Skip()
    {
        if (videoPlayer == null) return;

        videoPlayer.Stop();
        
        SceneManager.LoadScene(nextScene.name);
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
    }
    
}
