using System;
using UnityEngine;

public abstract class BaseProjectileComponent : MonoBehaviour
{
    // Each function has a boolean return for overwriting default behaviour
    // If return true then the default behaviour (eg. destroying bullet on collision) will be ignored

    // This class is meant to alter the projectile behaviour, for example
    // A bounce bullet would override the OnHit() method
    // An explosive bullet would override the OnBreak() method
    // A split bullet that breaks into smaller bullets would override the OnTravelling method
    // and so on

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
    public virtual bool OnHit(Projectile p, CharacterController target) // update this to include the player as well
    {
        return false;
    }

    // Called when the projectile breaks after collision
    public virtual bool OnBreak(Projectile p)
    {
        return false;
    }
}