using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class AbilityType : ScriptableObject
{
    public float force = 10f;
    public int damage = 150;
    public float range = 4f;
    public float cooldown = 10f;
    
    public float duration = 1f;
    public float delayTime = 0.5f;

    public bool isStaticAbility;
    public float tickRate = 0.5f;
    
    public bool isSticky;
    public bool canDamagePlayer;
    public bool canPathThroughWalls;
    
    public float Intenisty = 1f;
    public float delayffect = 0f;
    
    public AudioClip sound;
    public AudioClip babySound;
    public bool isLoopingAudio;
    
    public Sprite icon;
    
    public GameObject Effect;
}
