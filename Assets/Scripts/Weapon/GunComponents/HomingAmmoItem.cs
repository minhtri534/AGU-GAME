using UnityEngine;

public class HomingAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetProjectileSpeed(stats.GetProjectileSpeed() * 0.9f);
        stats.SetReloadTime(stats.GetReloadTime() * 1.05f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<HomingProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<HomingProjectileBehaviour>());
    }
}
