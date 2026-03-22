using UnityEngine;

public class GhostProjectileBehaviour : BaseProjectileComponent
{
    public override bool OnHit(Projectile p, CharacterController target)
    {
        // Ignore all collisions (walls) but still damage enemies.
        if (target != null && target.stats != null)
        {
            target.stats.TakeDamage(p.Damage);
        }

        return true;
    }
}
