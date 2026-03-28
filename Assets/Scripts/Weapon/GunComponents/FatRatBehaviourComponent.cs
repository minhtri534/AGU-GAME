using UnityEngine;

/// <summary>
/// Example class implementation of a gun component
/// </summary>
public class FatRatBehaviourComponent : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(2);
        stats.SetInaccuary(30);
        stats.SetProjectileSpeed(10);
        stats.SetProjectileLifeTime(1f);
        stats.SetNumberOfProjectiles(15);
        stats.SetReloadTime(2f);
        stats.SetManaRecoveryRate(100);
    }
}