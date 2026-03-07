using UnityEngine;

public class ChainLightningAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 0.95f);
        stats.SetReloadTime(stats.GetReloadTime() * 1.1f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<ChainLightningProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<ChainLightningProjectileBehaviour>());
    }
}
