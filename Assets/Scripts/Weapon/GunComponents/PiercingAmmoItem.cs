using UnityEngine;

public class PiercingAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        // Giảm tốc độ đạn còn 80%
        stats.SetProjectileSpeed(stats.GetProjectileSpeed() * 0.8f);
        // Tăng sát thương lên 120%
        stats.SetDamage(stats.GetDamage() * 1.2f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        p.addProjectileComponent(p.gameObject.AddComponent<PiercingProjectileBehaviour>());
    }
}