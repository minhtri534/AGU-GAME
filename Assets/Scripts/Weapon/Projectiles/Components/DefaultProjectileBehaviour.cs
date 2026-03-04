using System;
using UnityEngine;

public class DefaultProjectileBehaviour : BaseProjectileComponent
{
    // Fires bullet and self destroy after some time
    public override bool OnShot(Projectile p)
    {
        p.rb.linearVelocity = p.transform.forward * p.Speed; 
        return false;
    }
    
    // Deals damage on hit
    public override bool OnHit(Projectile p, EnemyController target) // update this to include the player as well
    {
        if (target != null && target.stats != null)
        {
            target.stats.TakeDamage(p.Damage);
            Debug.Log("Hit Enemy! HP còn: " + target.stats.CurrentHP);

            // update this code to be handled by the entity instead
            if (target.stats.IsDead)
            {
                target.Die();
                Debug.Log("Enemy Dead!");
            }
        }
        p.QueueOnBreak(); // Break after hit
        return false;
    }

    // Destroy the bullet
    public override bool OnBreak(Projectile p)
    {
        Destroy(p.gameObject);
        return false;
    }
}