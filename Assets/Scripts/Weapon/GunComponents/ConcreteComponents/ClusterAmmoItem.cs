using UnityEngine;

public class ClusterAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 0.9f);
        stats.SetReloadTime(stats.GetReloadTime() * 1.1f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<ClusterProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<ClusterProjectileBehaviour>());
    }
}
