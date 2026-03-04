using UnityEngine;

public class PiercingAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetProjectilSpeed(stats.projectileSpeed * 0.8f);
        stats.SetDamage(stats.damage * 1.2f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        p.addProjectileComponent(p.gameObject.AddComponent<PiercingProjectileBehaviour>());
    }
}