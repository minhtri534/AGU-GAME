using UnityEngine;

/// <summary>
/// Component type that changes the behaviour of projectiles.
/// Optionally changes the stats as well
/// </summary>
public abstract class BehaviourModifierComponent : BaseGunComponent
{
    /// <summary>
    /// Called whenever the gun component list is updated
    /// </summary>
    /// <param name="stats"></param>
    public virtual void ModifyStats(GunStats stats)
    {

    }
    /// <summary>
    /// Called whenever the projectile is fired
    /// </summary>
    /// <param name="p"></param>
    public virtual void AddComponentsToProjectile(Projectile p)
    {

    }

}
