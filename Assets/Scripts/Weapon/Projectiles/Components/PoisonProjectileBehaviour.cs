using UnityEngine;

public class PoisonProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float totalPoisonDamage = 6f;
    [SerializeField] private float poisonDuration = 3f;
    [SerializeField] private float tickInterval = 0.5f;

    public override bool OnHit(Projectile p, EnemyController target)
    {
        if (target == null)
        {
            return false;
        }

        var existing = target.GetComponent<DamageOverTimeEffect>();
        if (existing == null)
        {
            existing = target.gameObject.AddComponent<DamageOverTimeEffect>();
        }

        existing.Initialize(target, totalPoisonDamage, poisonDuration, tickInterval);
        return false; // let default handle direct damage + breaking
    }
}
