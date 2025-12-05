using System;
using TMPro;
using UnityEngine;

public class ScoreTest : MonoBehaviour
{
    [SerializeField] private float initialScore;
    public float score;
    [SerializeField] private float scaleFactor = 0.5f;
    [SerializeField] private float timeFactor = 60f;
    [SerializeField] private float currentTime;
    [SerializeField] TextMeshProUGUI scoreText;
    
    [SerializeField] private float scoreMultiplier = 100;
    [SerializeField] private float extraScore = 0;
    [SerializeField] private float extraScaleFactor = 0.5f;
    [SerializeField] private float extraTimeFactor = 120f;

    [SerializeField] private float totalScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = Time.time;
    }

    // Update is called once per frame
    void Update()
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
}
