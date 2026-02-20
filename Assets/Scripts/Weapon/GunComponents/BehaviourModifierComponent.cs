using UnityEngine;

// Component type that simply alters projectile stats
// and behaviours
public abstract class BehaviourModiferComponent : StatsModifierComponent
{
    // Called whenever the gun component list is updated
    public virtual void AddComponentsToProjectile(Projectile p)
    {
        
    }
}
