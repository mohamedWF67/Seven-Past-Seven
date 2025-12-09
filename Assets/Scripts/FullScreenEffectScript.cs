using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FullScreenEffectScript : MonoBehaviour
{
    public static FullScreenEffectScript instance;
    
    Image eyeBlink;
    [SerializeField] public float blinkTime = 2;
    [SerializeField] Material material;
    Color c;
    IEnumerator tweenCoroutine;


    enum FadeStyleEnum
    {
        Normal = 1,
        Eye = 2
    }
    [SerializeField] FadeStyleEnum fadeStyle = FadeStyleEnum.Eye;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        eyeBlink = GetComponent<Image>();
        material = eyeBlink.material;
        transform.SetAsFirstSibling();
    }
    
    IEnumerator StartTween()
    {
        transform.SetAsLastSibling();
        if (fadeStyle == FadeStyleEnum.Normal)
        {
            material.SetFloat("_VignettePower", 0);
            LeanTween.value(gameObject, 0, 1, blinkTime)
                .setEase(LeanTweenType.easeInOutCubic)
                .setLoopPingPong(1)
                .setOnUpdate(val =>
                {
                    c = eyeBlink.color;
                    c.a = val;
                    eyeBlink.color = c;
                });
        }
        else
        {
            c = eyeBlink.color; c.a = 1; eyeBlink.color = c;
            LeanTween.value(gameObject, 1, 0, blinkTime)
                .setEase(LeanTweenType.easeInOutCubic)
                .setLoopPingPong(1)
                .setOnUpdate(val =>
                {
                    material.SetFloat("_VignettePower", val);
                });
        }
        yield return new WaitForSeconds(blinkTime * 2);
        //Debug.Log("Done");
        transform.SetAsFirstSibling();
        tweenCoroutine = null;
    }

    public void Blink()
    {
        if (tweenCoroutine == null)
        {
            //Debug.Log("tweening");
            tweenCoroutine = StartTween();
            StartCoroutine(tweenCoroutine);
        }
        else
        {
            //Debug.Log("re tweening blink");
            transform.SetAsFirstSibling();
            StopCoroutine(tweenCoroutine);
            tweenCoroutine = null;
            tweenCoroutine = StartTween();
            StartCoroutine(tweenCoroutine);
        }
    }
}
