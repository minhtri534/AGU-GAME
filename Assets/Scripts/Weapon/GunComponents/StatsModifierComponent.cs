using UnityEngine;

// Component type that simply alters projectile stats
// and behaviours
public abstract class StatsModifierComponent : BaseGunComponent
{
    // Called whenever the gun component list is updated
    public virtual void ModifyStats(GunStats stats)
    {
        
    }
}
