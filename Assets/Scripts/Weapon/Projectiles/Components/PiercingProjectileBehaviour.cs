using UnityEngine;
public class PiercingProjectileBehaviour : BaseProjectileComponent
{
    public override bool OnHit (Projectile p, EnemyController target) 
    {
        if (target != null)
        {
            target.TakeDamage(p.damage);
        }
        return true;
    }
}