using System.Collections;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// The gun, can be used by either the player or the enemy
/// </summary>
public class Gun : MonoBehaviourPun
{
    [Header("Settings")]
    public bool isEnemyWeapon = false;
    public Transform firePoint;     // Kéo GameObject vị trí đầu nòng súng vào đây
    public GunInput GunInput;
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
        stats = new();
        inventory = new(stats);
    }

    void Start()
    {
        if (isEnemyWeapon)
        {
            GunInput = gameObject.AddComponent<GunInputEnemy>();
            AimTarget = new GunAimTargetEnemy();
        }
        else
        {
            // If this object is networked by Photon, only the local owner should handle input
            var pv = GetComponent<PhotonView>();
            if (pv == null || pv.IsMine)
            {
                GunInput = gameObject.AddComponent<GunInput>();
                AimTarget = new GunAimTargetPlayer();
            }
            else
            {
                // Remote players should not process local input — leave GunInput null so Update() skips
                GunInput = null;
                AimTarget = null;
            }
        }
        StartCoroutine(RestoreMana());
    }

    void Update()
    {
        if (GunInput == null || AimTarget == null || firePoint == null) return;

        // Fallback if no type modifier component is equipped
        if (inventory.GetTypeModifierComponent() != null && inventory.GetTypeModifierComponent().OverrideDefaultGunControl)
        {
            inventory.GetTypeModifierComponent().ManageGun(GunInput.GetInput(), this);
        }
        else
        {
            if (GunInput.GetInput() == GunInputState.JustPressed)
            {
                Shoot(AimTarget.Aim(firePoint.position));
            }
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

        if ((int)stats.GetManaPerShot() > gameObject.GetComponent<CharacterController>().stats.CurrentMP)
        {
            return;
        }

        gameObject.GetComponent<CharacterController>().stats.UseMP((int)stats.GetManaPerShot());

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
        GameObject bulletObj = null;
        var pv = GetComponent<PhotonView>();
        // If connected to Photon and this gun is owned by local player, instantiate projectile via Photon so other clients see it.
        if (PhotonNetwork.IsConnected && pv != null && pv.IsMine)
        {
            // PhotonNetwork.Instantiate requires the prefab to be in a Resources folder.
            // Pass basic projectile data so remote clients can configure the projectile identically.
            string prefabName = stats.ProjectilePrefab.name;
            object[] instData = new object[] {
                pv.ViewID,
                (float)stats.GetDamage(),
                (float)stats.GetProjectileSpeed(),
                (float)stats.GetProjectileLifeTime(),
                (float)stats.GetProjectileSize(),
                isEnemyWeapon
            };
            try
            {
                bulletObj = PhotonNetwork.Instantiate(prefabName, firePoint.position, rotation, 0, instData);
            }
            catch
            {
                // Fallback to local instantiate if prefab not found in Resources or instantiate fails
                bulletObj = Instantiate(stats.ProjectilePrefab, firePoint.position, rotation);
            }
        }
        else
        {
            // Not networked or not the owner — create local projectile (for enemies or non-networked play)
            bulletObj = Instantiate(stats.ProjectilePrefab, firePoint.position, rotation);
        }

        // Configure collision layers and visuals (creator will also set these; remote clients get equivalent data from instantiationData)
        var col = bulletObj.GetComponent<Collider>();
        if (col != null)
        {
            if (isEnemyWeapon)
            {
                col.excludeLayers += LayerMask.GetMask("Enemy");
                var mr = bulletObj.GetComponent<MeshRenderer>();
                if (mr != null) mr.material = Resources.Load<Material>("Materials/BulletEnemy");
            }
            else
            {
                col.excludeLayers += LayerMask.GetMask("Player");
            }
        }

        Projectile bullet = bulletObj.GetComponent<Projectile>();

        if (bullet != null)
        {
            bullet.Damage = stats.GetDamage();
            bullet.Speed = stats.GetProjectileSpeed();
            bullet.LifeTime = stats.GetProjectileLifeTime();
            bullet.Size = stats.GetProjectileSize();
            bullet.ProjectileOwner = this;
            bullet.GunStats = stats;
            // Add projectile components (these will run only on the creator for now)
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

    private IEnumerator RestoreMana()
    {
        yield return new WaitForSeconds(stats.GetReloadTime());
        gameObject.GetComponent<CharacterController>().stats.UseMP(-stats.GetManaRecoveryRate());
        StartCoroutine(RestoreMana());
    }
}
