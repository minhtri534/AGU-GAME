using UnityEngine;

public class ChainLightningProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float chainRadius = 6f;
    [SerializeField] private int maxExtraTargets = 3;
    [SerializeField] private float extraDamageMultiplier = 0.5f;

    public override bool OnHit(Projectile p, EnemyController target)
    {
        if (target == null)
        {
            return false;
        }

        var origin = target.transform.position;
        var colliders = Physics.OverlapSphere(origin, chainRadius);

        var hits = 0;
        foreach (var col in colliders)
        {
            if (hits >= maxExtraTargets)
            {
                break;
            }

            var enemy = col.GetComponentInParent<EnemyController>();
            if (enemy == null || enemy == target || enemy.stats == null)
            {
                continue;
            }

            enemy.stats.TakeDamage(p.Damage * extraDamageMultiplier);
            if (enemy.stats.IsDead)
            {
                enemy.Die();
            }

            hits++;
        }

        return false; // let default handle main target damage + break
    }
}
