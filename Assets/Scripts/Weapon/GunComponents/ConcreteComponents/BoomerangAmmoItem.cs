using UnityEngine;

public class BoomerangAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetProjectileSpeed(stats.GetProjectileSpeed() * 0.95f);
        stats.SetProjectileLifeTime(stats.GetProjectileLifeTime() * 1.2f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<BoomerangProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<BoomerangProjectileBehaviour>());
    }
}
