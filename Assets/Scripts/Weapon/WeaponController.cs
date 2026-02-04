using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class WeaponController : MonoBehaviour
{
    public WeaponData weaponData;
    public Transform muzzle; // spawn position for projectiles
    public Transform aimPivot; // rotation pivot (usually attached to player)

    [HideInInspector] public int currentAmmo;

    // dynamic/current stats after applying attachments
    private float currentDamage;
    private float currentFireRate;
    private float currentProjectileSpeed;
    private float currentSpread;
    private float currentReloadTime;
    private int currentMagazineSize;
    private int currentPellets;
    private bool currentIsAutomatic;

    private WeaponState currentState;
    private System.Collections.Generic.List<AttachmentData> attachments = new System.Collections.Generic.List<AttachmentData>();

    private float lastFireTime = -999f;

    void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogWarning("WeaponData not assigned on " + gameObject.name);
            return;
        }
        // initialize base + derived stats
        RecalculateStats();
        currentAmmo = currentMagazineSize;
        currentState = new WeaponIdleState(this);
    }

    void Update()
    {
        RotateToMouse();
        currentState.HandleInput();
        currentState.StateUpdate();
    }

    // Recompute current stats from base weaponData + attachments
    public void RecalculateStats()
    {
        currentDamage = weaponData.damage;
        currentFireRate = weaponData.fireRate;
        currentProjectileSpeed = weaponData.projectileSpeed;
        currentSpread = weaponData.spreadAngle;
        currentReloadTime = weaponData.reloadTime;
        currentMagazineSize = weaponData.magazineSize;
        currentPellets = weaponData.pellets;
        currentIsAutomatic = weaponData.isAutomatic;

        foreach (var att in attachments)
        {
            if (att == null) continue;
            switch (att.type)
            {
                case AttachmentData.AttachmentType.Damage:
                    currentDamage += att.value;
                    break;
                case AttachmentData.AttachmentType.FireRateMultiplier:
                    currentFireRate *= Mathf.Max(0.01f, att.value);
                    break;
                case AttachmentData.AttachmentType.MagazineIncrease:
                    currentMagazineSize += Mathf.Max(0, att.intValue);
                    break;
                case AttachmentData.AttachmentType.ReloadSpeedMultiplier:
                    currentReloadTime *= Mathf.Max(0.01f, att.value);
                    break;
                case AttachmentData.AttachmentType.ProjectileSpeedMultiplier:
                    currentProjectileSpeed *= Mathf.Max(0.01f, att.value);
                    break;
                case AttachmentData.AttachmentType.SpreadReduction:
                    currentSpread = Mathf.Max(0f, currentSpread - att.value);
                    break;
                case AttachmentData.AttachmentType.PelletsIncrease:
                    currentPellets += Mathf.Max(0, att.intValue);
                    break;
                case AttachmentData.AttachmentType.AutoEnable:
                    if (att.enable) currentIsAutomatic = true;
                    break;
            }
        }
    }

    public void SetState(WeaponState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }


    public void TryStartFiring()
    {
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }
        SetState(new WeaponFiringState(this));
    }

    public void TryFireOnce()
    {
        float interval = GetFireInterval();
        if (Time.time - lastFireTime < interval) return;
        if (currentAmmo <= 0) return;

        FirePrimary();
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        lastFireTime = Time.time;
    }

    // single unified fire behaviour; respects currentPellets/spread/currentProjectileSpeed/currentDamage
    private void FirePrimary()
    {
        if (muzzle == null) return;
        var baseDir = GetAimDirection();
        for (int i = 0; i < Mathf.Max(1, currentPellets); i++)
        {
            var angle = Random.Range(-currentSpread * 0.5f, currentSpread * 0.5f);
            var rotated = Quaternion.AngleAxis(angle, Vector3.up) * baseDir;
            SpawnProjectile(rotated);
        }
    }

    public void StartReload()
    {
        SetState(new WeaponReloadingState(this));
    }

    public float GetFireInterval()
    {
        return 1f / Mathf.Max(0.0001f, currentFireRate);
    }

    public bool CurrentIsAutomatic => currentIsAutomatic;

    // Attachments API
    public void AddAttachment(AttachmentData att)
    {
        if (att == null) return;
        attachments.Add(att);
        RecalculateStats();
        // optionally refill ammo when magazine size increases
        currentAmmo = Mathf.Min(currentAmmo, currentMagazineSize);
    }

    public void RemoveAttachment(AttachmentData att)
    {
        if (att == null) return;
        attachments.Remove(att);
        RecalculateStats();
        currentAmmo = Mathf.Min(currentAmmo, currentMagazineSize);
    }

    public Vector3 GetAimDirection()
    {
        if (muzzle == null) return transform.forward;
        return (muzzle.forward).normalized;
    }

    public void SpawnProjectile(Vector3 direction)
    {
        if (weaponData.projectilePrefab == null || muzzle == null)
        {
            Debug.LogWarning("Projectile prefab or muzzle not assigned on " + gameObject.name);
            return;
        }

        var obj = Instantiate(weaponData.projectilePrefab, muzzle.position, Quaternion.LookRotation(direction));
        var proj = obj.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.speed = currentProjectileSpeed;
            proj.damage = currentDamage;
        }
        else
        {
            // best-effort: add velocity if rigidbody present
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * currentProjectileSpeed;
            }
        }
    }

    private void RotateToMouse()
    {
        if (aimPivot == null || Camera.main == null) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.up, aimPivot.position);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Vector3 dir = hit - aimPivot.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                var target = Quaternion.LookRotation(dir);
                aimPivot.rotation = Quaternion.Slerp(aimPivot.rotation, target, 20f * Time.deltaTime);
            }
        }
    }
}
