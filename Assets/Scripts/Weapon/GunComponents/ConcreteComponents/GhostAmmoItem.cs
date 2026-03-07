using UnityEngine;

public class GhostAmmoItem : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetProjectileLifeTime(stats.GetProjectileLifeTime() * 0.8f);
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        if (p.GetComponent<GhostProjectileBehaviour>() != null)
        {
            return;
        }

        p.addProjectileComponent(p.gameObject.AddComponent<GhostProjectileBehaviour>());
    }
}
