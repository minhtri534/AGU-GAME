using UnityEngine;

/// <summary>
/// Stats for the gun to be used when creating a new projectile
/// </summary>
public class GunStats
{
    public readonly float BaseDamage = 10;
    public readonly float BaseProjectileSpeed = 7;
    public readonly float BaseProjectileLifeTime = 5; // seconds
    public readonly float BaseReloadTime = 1; // 1 second before the next shot can be fired
    public readonly float BaseInaccuracy = 10; // bullet fired will have an offset between -10 and 10 degrees
    public readonly int BaseNumberOfProjectiles = 1; // 1 bullet per shot
    public readonly ShotType BaseShotType = ShotType.Normal;
    public readonly GameObject BaseProjectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/Bullet");
    // These stats are updated using base stats whenever a new component is added
    private float damage;
    private float projectileSpeed;
    private float projectileLifeTime;
    private float reloadTime;
    private float inaccuracy;
    private int numberOfProjectiles;
    public ShotType ShotType;
    public GameObject ProjectilePrefab;

    public void SetDamage(float value)
    {
        if (value < 0.1)
        {
            value = 0.1f;
        }
        damage = value;
    }
    public float GetDamage()
    {
        return damage;
    }
    public void SetProjectileSpeed(float value)
    {
        if (value < 0.1)
        {
            value = 0.1f;
        }
        projectileSpeed = value;
    }
    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }
    public void SetProjectileLifeTime(float value)
    {
        if (value < 0.1)
        {
            value = 0.1f;
        }
        projectileLifeTime = value;
    }
    public float GetProjectileLifeTime()
    {
        return projectileLifeTime;
    }
    public void SetReloadTime(float value)
    {
        if (value < 0.1)
        {
            reloadTime = 0.1f;
        }
        reloadTime = value;
    }
    public float GetReloadTime()
    {
        return reloadTime;
    }
    public void SetInaccuary(float value)
    {
        if (value < 0)
        {
            inaccuracy = 0;
        }
        else if (value > 360)
        {
            inaccuracy = 360;
        }
        inaccuracy = value;
    }
    public float GetInaccuracy()
    {
        return inaccuracy;
    }
    public void SetNumberOfProjectiles(int value)
    {
        if (value < 1)
        {
            value = 1;
        }
        numberOfProjectiles = value;
    }
    public int GetNumberOfProjectiles()
    {
        return numberOfProjectiles;
    }

    public GunStats()
    {
        ResetStats();
    }

    /// <summary>
    /// Reset all stats to the base stats
    /// </summary>
    /// <remarks>
    /// Meant to be called whenever the gun component list is updated
    /// </remarks>
    public void ResetStats()
    {
        damage = BaseDamage;
        projectileSpeed = BaseProjectileSpeed;
        projectileLifeTime = BaseProjectileLifeTime;
        reloadTime = BaseReloadTime;
        inaccuracy = BaseInaccuracy;
        numberOfProjectiles = BaseNumberOfProjectiles;
        ShotType = BaseShotType;
        ProjectilePrefab = BaseProjectilePrefab;
    }
}

public enum ShotType
{
    Normal, // fires projectiles normally
    Multishot, // projectiles fired are spread out evenly within inaccuracy range
    RapidFire, // fires multiple projectiles in rapid succession
}