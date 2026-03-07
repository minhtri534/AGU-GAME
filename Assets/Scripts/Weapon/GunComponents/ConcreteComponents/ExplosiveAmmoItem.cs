using UnityEngine;

public class ExplosiveAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetReloadTime(stats.GetReloadTime() * 1.15f);
        stats.SetProjectileSpeed(stats.GetProjectileSpeed() * 0.9f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<ExplosiveProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<ExplosiveProjectileBehaviour>());
    }
}
