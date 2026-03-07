using UnityEngine;

public class PoisonAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 0.9f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<PoisonProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<PoisonProjectileBehaviour>());
    }
}
