using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Shot Type", menuName = "Shooter/Shot Type")]
public class ShotType : ScriptableObject
{
    [SerializeField] public int damage = 10; 
    [SerializeField] public float fireRate = 0.5f;
    [SerializeField] public float speed = 10f;
    [SerializeField] public float destroyTime = 1f;

    [SerializeField] public bool canPathThroughWalls;
    [SerializeField] public bool canPathThroughEnemies;
    
    public GameObject bullet;
    [SerializeField] public Sprite icon;
    [SerializeField] public bool isLoopingAudio;
    [SerializeField] public AudioClip sound;
    public float DPS => damage / fireRate;
    public float waitTime => 0.25f / speed;
    
}
