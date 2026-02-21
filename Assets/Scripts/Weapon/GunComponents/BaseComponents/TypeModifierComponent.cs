using UnityEngine;

// Component type that changes the projectile type
public abstract class TypeModifierComponent : BehaviourModifierComponent
{
    /// <summary>
    /// Called every frame by the gun.
    /// Essentially overrides the gun's behaviour
    /// </summary>
    /// <param name="state"></param>
    /// <param name="g"></param>
    public virtual void ManageGun(GunInputState state, Gun g)
    {
        
    }
}
