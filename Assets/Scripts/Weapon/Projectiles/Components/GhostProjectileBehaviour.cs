using UnityEngine;

public class GhostProjectileBehaviour : BaseProjectileComponent
{
    public override bool OnHit(Projectile p, EnemyController target)
    {
        // Ignore all collisions (walls) but still damage enemies.
        if (target != null && target.stats != null)
        {
            target.stats.TakeDamage(p.Damage);
            if (target.stats.IsDead)
            {
                target.Die();
            }
        }

        return true;
    }
}
