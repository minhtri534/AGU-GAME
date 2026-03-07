using UnityEngine;

public class AcceleratingAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetProjectileSpeed(stats.GetProjectileSpeed() * 0.85f);
        stats.SetProjectileLifeTime(stats.GetProjectileLifeTime() * 1.1f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<AcceleratingProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<AcceleratingProjectileBehaviour>());
    }
}
