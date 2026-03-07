using UnityEngine;

public class ExplosiveProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private float damageMultiplier = 1f;

    public override bool OnHit(Projectile p, EnemyController target)
    {
        // Explode on any collision (enemy or wall)
        p.QueueOnBreak();
        return true;
    }

    public override bool OnBreak(Projectile p)
    {
        var hitPos = p.transform.position;
        var colliders = Physics.OverlapSphere(hitPos, radius);

        foreach (var col in colliders)
        {
            var enemy = col.GetComponentInParent<EnemyController>();
            if (enemy == null || enemy.stats == null)
            {
                continue;
            }

            enemy.stats.TakeDamage(p.Damage * damageMultiplier);
            if (enemy.stats.IsDead)
            {
                enemy.Die();
            }
        }

        return true;
    }
}
