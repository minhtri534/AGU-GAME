using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // BẮT BUỘC: Thêm dòng này để dùng hệ thống Input mới

/// <summary>
/// The gun, can be used by either the player or the enemy
/// </summary>
public class Gun : MonoBehaviour
{
    [Header("Settings")]
    public bool isEnemyWeapon = false;
    public Transform firePoint;     // Kéo GameObject vị trí đầu nòng súng vào đây
    private GunInput gunInput;
    public GunStats stats;
    public GunComponentInventory inventory;
    public IGunAimTarget AimTarget;
    private bool canFire = true;

    public GunStats GetStats()
    {
        return stats;
    }

    void Awake()
    {
        stats = new GunStats();
        inventory = new GunComponentInventory(stats);
        if (isEnemyWeapon)
        {
            gunInput = gameObject.AddComponent<GunInputEnemy>();
            AimTarget = new GunAimTargetEnemy();
        }
        else
        {
            gunInput = gameObject.AddComponent<GunInput>();
            AimTarget = new GunAimTargetPlayer();
        }
    }

    void Update()
    {
        // Fallback if no type modifier component is equipped
        if (inventory.GetTypeModifierComponent() == null)
        {
            if (gunInput.GetInput() == GunInputState.JustPressed)
            {
                Shoot(AimTarget.Aim(firePoint.position));
            }
        }
        else
        {
            inventory.GetTypeModifierComponent().ManageGun(gunInput.GetInput(), this);
        }

    }
    /// <summary>
    /// Fires projectiles
    /// </summary>
    /// <param name="rotation"></param>
    public void Shoot(Quaternion rotation)
    {
        if (firePoint == null) return;

        if (!canFire)
        {
            return;
        }
        switch (stats.ShotType)
        {
            case ShotType.Normal:
                // Apply inaccuracy to each projectiles
                for (int i = 0; i < stats.GetNumberOfProjectiles(); i++)
                {
                    var newRotation = Quaternion.AngleAxis(Random.Range(-stats.GetInaccuracy(), stats.GetInaccuracy()), Vector3.up) * rotation;
                    CreateProjectile(newRotation);
                }
                break;
            case ShotType.Multishot:
                // Apply spread to each projectiles
                for (int i = 0; i < stats.GetNumberOfProjectiles(); i++)
                {
                    var newRotation = Quaternion.AngleAxis(-stats.GetInaccuracy() + i * 2 * stats.GetInaccuracy() / (stats.GetNumberOfProjectiles() - 1), Vector3.up) * rotation;
                    CreateProjectile(newRotation);
                }
                break;
            case ShotType.RapidFire:
                for (int i = 0; i < stats.GetNumberOfProjectiles(); i++)
                {
                    var newRotation = Quaternion.AngleAxis(Random.Range(-stats.GetInaccuracy(), stats.GetInaccuracy()), Vector3.up) * rotation;

                    // create projectiles in coroutine to delay firing between each projectile
                    StartCoroutine(DelayRapidFire(newRotation, i * stats.GetReloadTime() / stats.GetNumberOfProjectiles() * 0.5f));
                }
                break;
        }
        StartCoroutine(Reload());
    }
    private void CreateProjectile(Quaternion rotation)
    {
        GameObject bulletObj = Instantiate(stats.ProjectilePrefab, firePoint.position, rotation);

        // Make projectiles ignore character collisions
        if (isEnemyWeapon)
        {
            bulletObj.GetComponent<Collider>().excludeLayers += LayerMask.GetMask("Enemy");
        } else
        {
            bulletObj.GetComponent<Collider>().excludeLayers += LayerMask.GetMask("Player");
        }
        

        Projectile bullet = bulletObj.GetComponent<Projectile>();

        if (bullet != null)
        {
            // update this part later to add all the projectile components
            // and change the player influence to be a multiplier instead
            //float playerDamage = player.GetStats().Damage;
            //bullet.SetDamage(playerDamage);
            bullet.Damage = stats.GetDamage();
            bullet.Speed = stats.GetProjectileSpeed();
            bullet.LifeTime = stats.GetProjectileLifeTime();
            bullet.Size = stats.GetProjectileSize();
            // Add projectile components
            inventory.GetTypeModifierComponent()?.AddComponentsToProjectile(bullet);
            for (int i = 0; i < inventory.InventorySize; i++)
            {
                inventory.GetBehaviourModifierComponent(i)?.AddComponentsToProjectile(bullet);
            }
        }
    }
    private IEnumerator Reload()
    {
        canFire = false;
        yield return new WaitForSeconds(stats.GetReloadTime());
        canFire = true;
    }

    private IEnumerator DelayRapidFire(Quaternion rotation, float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateProjectile(rotation);
    }
}
