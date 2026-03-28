using UnityEngine;

/// <summary>
/// Example class implementation of a gun component
/// </summary>
public class ShortRatBehaviourComponent : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(15);
        stats.SetProjectileSpeed(10);
        stats.SetProjectileLifeTime(0.3f);
        stats.SetReloadTime(0.5f);
        stats.SetManaRecoveryRate(100);
        stats.ProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/MeleeAttack");
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        p.addProjectileComponent(p.gameObject.AddComponent<MeleeProjectileBehaviour>());
        p.addProjectileComponent(p.gameObject.AddComponent<GhostProjectileBehaviour>());
    }
}