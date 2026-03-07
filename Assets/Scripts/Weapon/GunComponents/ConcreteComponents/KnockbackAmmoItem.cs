using UnityEngine;

public class KnockbackAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetReloadTime(stats.GetReloadTime() * 1.05f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<KnockbackProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<KnockbackProjectileBehaviour>());
    }
}
