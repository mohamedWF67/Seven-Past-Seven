using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;
    
    [SerializeField]private float shakeDuration;
    [SerializeField]private float shakeIntensity;
    [SerializeField]private float shakeFrequency;
    
    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        noise = GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(DoShake());
        }
    }

    IEnumerator DoShake()
    {
        noise.AmplitudeGain = shakeIntensity;
        noise.FrequencyGain = shakeFrequency;
        
        yield return new WaitForSeconds(shakeDuration);
        
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }
}
