using UnityEngine;

[CreateAssetMenu(fileName = "ACT_Data", menuName = "Scriptable Objects/ACT_Data")]
public class ACT_Data : ScriptableObject
{
    public ShootingScript.Shooter CurrentAbilityStack;

    public int maxHealth;
    public int maxHearts;
    public float enduranceModifier;

    public bool canFire;
    public bool canDash;
    public bool canDoubleJump;
    public bool canWallJump;
    
}
