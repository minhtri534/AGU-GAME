using UnityEngine;

// Component type that simply alters projectile stats
// and behaviours
public abstract class TypeModiferComponent : BehaviourModiferComponent
{
    // Called whenever the gun component list is updated
    public virtual void ManageGun(Gun g)
    {
        
    }
}
