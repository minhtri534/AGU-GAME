using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // BẮT BUỘC: Thêm dòng này để dùng hệ thống Input mới

/// <summary>
/// The gun, meant to be used by players
/// </summary>
public class Gun : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab; // Kéo Prefab viên đạn vào đây
    public Transform firePoint;     // Kéo GameObject vị trí đầu nòng súng vào đây
    private GunInput gunInput;
    private PlayerController player;
    public GunStats stats;
    public GunComponentInventory inventory;
    private bool canFire = true;

    public GunStats GetStats()
    {
        return stats;
    }

    void Awake()
    {
        player = GetComponent<PlayerController>();
        gunInput = gameObject.AddComponent<GunInput>();
        stats = new GunStats();
        inventory = new GunComponentInventory(stats);
    }

    void Update()
    {
        // Fallback if no type modifier component is equipped
        if (inventory.GetTypeModifierComponent() == null)
        {
            if (gunInput.GetInput() == GunInputState.JustPressed)
            {
                Shoot(Aim());
            }
        }
        else
        {
            inventory.GetTypeModifierComponent().ManageGun(gunInput.GetInput(), this);
        }

    }
    /// <summary>
    /// Returns the rotation to the mouse position
    /// </summary>
    /// <remarks>
    /// TODO: create a child class for enemies that overrides this function to aim at players instead
    /// </remarks>
    /// <returns></returns>
    public Quaternion Aim()
    {
        if (Camera.main == null) return Quaternion.identity; // Kiểm tra camera

        // SỬA LỖI: Dùng Mouse.current.position thay vì Input.mousePosition
        Vector2 mouseScreenPos = Vector2.zero;
        if (Mouse.current != null)
        {
            mouseScreenPos = Mouse.current.position.ReadValue();
        }

        // 1. Tính toán điểm người chơi đang click chuột
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        // Tạo mặt phẳng ảo ngang tầm với súng (để chuột luôn nằm trên mặt phẳng này)
        Plane plane = new(Vector3.up, firePoint.position);

        Vector3 hitPoint = Vector3.zero;
        if (plane.Raycast(ray, out float enter))
        {
            hitPoint = ray.GetPoint(enter);
        }
        else
        {
            // Fallback nếu click ra ngoài trời
            hitPoint = ray.GetPoint(50f);
        }

        // 2. Tính hướng từ súng tới điểm click
        Vector3 direction = hitPoint - firePoint.position;
        direction.y = 0; // Giữ đạn bay ngang, không cắm đầu xuống đất

        if (direction == Vector3.zero) return Quaternion.identity;

        return Quaternion.LookRotation(direction);
    }
    /// <summary>
    /// Fires projectiles
    /// </summary>
    /// <param name="rotation"></param>
    public void Shoot(Quaternion rotation)
    {
        if (bulletPrefab == null || firePoint == null) return;

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
