using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    
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

        Instance = this;
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(DoShake());
        }else
            shakeCoroutine = StartCoroutine(DoShake());
        
    }
    
    public void Shake(float shakeAmount,float shakeFreq,float duration = 0)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = StartCoroutine(DoShake(shakeAmount,shakeFreq,duration));
        }else
            shakeCoroutine = StartCoroutine(DoShake(shakeAmount,shakeFreq,duration));
    }

    IEnumerator DoShake()
    {
        Debug.Log("Heez ya wiz");
        noise.AmplitudeGain = shakeIntensity;
        noise.FrequencyGain = shakeFrequency;
        
        yield return new WaitForSeconds(shakeDuration);
        
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }

    IEnumerator DoShake(float shakeAmount,float shakeFreq,float duration = 0)
    {
        Debug.Log("Heez ya wiz");
        noise.AmplitudeGain = shakeAmount;
        noise.FrequencyGain = shakeFreq;
        
        yield return new WaitForSeconds(duration != 0 ? duration : shakeDuration);
        
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }
}
