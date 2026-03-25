using Unity.VisualScripting;
using UnityEngine;

public class MeleeAmmoItem : TypeModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetProjectileLifeTime(0.2f);
        stats.SetProjectileSpeed(10);
        stats.ProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/MeleeAttack");
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        p.addProjectileComponent(p.AddComponent<MeleeProjectileBehaviour>());
        p.addProjectileComponent(p.AddComponent<GhostProjectileBehaviour>());
    }
}