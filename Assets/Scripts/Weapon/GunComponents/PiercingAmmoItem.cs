using UnityEngine;

public class PiercingAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        float newSpeed = stats.GetProjectileSpeed() * 0.8f;
        float newDamage = stats.GetDamage() * 1.2f;

        stats.SetProjectileSpeed(newSpeed);
        stats.SetDamage(newDamage);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        p.addProjectileComponent(p.gameObject.AddComponent<PiercingProjectileBehaviour>());
    }
}