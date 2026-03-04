using UnityEngine;

public class PiercingProjectileBehaviour : BaseProjectileComponent
{
    public override bool OnHit(Projectile p, EnemyController target)
    {
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