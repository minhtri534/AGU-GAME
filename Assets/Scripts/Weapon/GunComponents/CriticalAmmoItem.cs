using UnityEngine;

public class CriticalAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        // Slightly lower base damage, crit behaviour can spike
        stats.SetDamage(stats.GetDamage() * 0.95f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<CriticalHitProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<CriticalHitProjectileBehaviour>());
    }
}
