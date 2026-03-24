using System;
using UnityEngine;

public class DefaultProjectileBehaviour : BaseProjectileComponent
{
    // Fires bullet and self destroy after some time
    public override bool OnShot(Projectile p)
    {
        p.rb.linearVelocity = p.transform.forward * p.Stats.GetProjectileSpeed(); 
        return false;
    }
    
    // Deals damage on hit
    public override bool OnHit(Projectile p, CharacterController target)
    {
        if (target != null && target.stats != null)
        {
            target.stats.TakeDamage(p.Stats.GetDamage());
            Debug.Log("Hit " + target + "! HP còn: " + target.stats.CurrentHP);
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