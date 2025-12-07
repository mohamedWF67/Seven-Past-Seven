using UnityEngine;

[CreateAssetMenu(fileName = "ACT_Data", menuName = "Scriptable Objects/ACT_Data")]
public class ACT_Data : ScriptableObject
{
    public enum Shooter
    {
        Clouden = 0,
        Blazzy = 1,
        Echo = 2,
        Aira = 3,
        Mallow = 4,
        Racci = 5
    }
    public Shooter CurrentAbilityStack;

    public int maxHealth;
    public int maxHearts;
    public float enduranceModifier;

    public bool canFire;
    public bool canDash;
    public bool canDoubleJump;
    public bool canWallJump;
    
}
