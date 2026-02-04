using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Base stats")]
    public string weaponName = "New Weapon";
    public GameObject projectilePrefab;
    public float damage = 10f;
    public float fireRate = 4f; // shots per second
    public int magazineSize = 12;
    public float reloadTime = 1.5f;
    public float projectileSpeed = 20f;
    public float spreadAngle = 5f; // degrees
    public int pellets = 1; // number of projectiles per shot (default 1)
    public bool isAutomatic = false;
}
