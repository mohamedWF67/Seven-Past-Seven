using UnityEngine;

[CreateAssetMenu(fileName = "AudioScene", menuName = "AudioScene")]
public class AudioScene : ScriptableObject
{
    public AudioClip audioClip;
    public float volume = 1f;
}
