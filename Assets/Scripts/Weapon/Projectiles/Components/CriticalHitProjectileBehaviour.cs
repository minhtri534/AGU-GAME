using UnityEngine;

public class CriticalHitProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField, Range(0f, 1f)] private float critChance = 0.25f;
    [SerializeField] private float critMultiplier = 2f;

    public override bool OnShot(Projectile p)
    {
        if (Random.value <= critChance)
        {
            p.Stats.SetDamage(p.Stats.GetDamage() * Mathf.Max(1f, critMultiplier));
        }
        return false;
    }
}
