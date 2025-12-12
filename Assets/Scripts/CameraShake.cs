using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine shakeCoroutine;
    
    [SerializeField]private float shakeDuration = 0.5f;
    [SerializeField]private float shakeIntensity = 5;
    [SerializeField]private float shakeFrequency = 1;
    
    private void Awake()
    {
        noise = GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        Instance = this;
    }

    public void Shake()
    {
        Shake(shakeIntensity,shakeFrequency,shakeDuration);
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
        DoShake(shakeIntensity,shakeFrequency,shakeDuration);
        yield return new WaitForSeconds(shakeDuration);
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
