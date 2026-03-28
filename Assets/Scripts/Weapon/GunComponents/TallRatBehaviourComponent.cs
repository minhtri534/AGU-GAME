using UnityEngine;

/// <summary>
/// Example class implementation of a gun component
/// </summary>
public class TallRatBehaviourComponent : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(20);
        stats.SetInaccuary(5);
        stats.SetProjectileSpeed(20);
        stats.SetReloadTime(3f);
        stats.SetManaRecoveryRate(100);
    }
}