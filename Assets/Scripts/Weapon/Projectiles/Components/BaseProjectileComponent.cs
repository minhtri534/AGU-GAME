using System;
using UnityEngine;

public abstract class BaseProjectileComponent : MonoBehaviour
{
    // Each function has a boolean return for overwriting default behaviour
    // If return true then the default behaviour (eg. destroying bullet on collision) will be ignored


    // Called whenever the projectile has just been fired
    public virtual bool OnShot(Projectile p)
    {
        return false;
    }
    
    // Called every frame as the projectile is travelling
    public virtual bool OnTraveling(Projectile p)
    {
        return false;
    }

    // Called when the projectile collides
    public virtual bool OnHit(Projectile p, EnemyController target) // update this to include the player as well
    {
        return false;
    }

    // Called when the projectile breaks after collision
    public virtual bool OnBreak(Projectile p)
    {
        return false;
    }
}