using UnityEngine;

public class HomingProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float homingRange = 12f;
    [SerializeField] private float turnSpeedDegreesPerSecond = 360f;
    [SerializeField] private float retargetInterval = 0.15f;

    private EnemyController currentTarget;
    private float retargetTimer;

    public override bool OnTraveling(Projectile p)
    {
        retargetTimer -= Time.deltaTime;
        if (retargetTimer <= 0f)
        {
            retargetTimer = retargetInterval;
            currentTarget = FindNearestTarget(p.transform.position);
        }

        if (currentTarget == null)
        {
            return false;
        }

        var toTarget = currentTarget.transform.position - p.transform.position;
        toTarget.y = 0f;
        if (toTarget.sqrMagnitude < 0.0001f)
        {
            return false;
        }

        var desiredDir = toTarget.normalized;
        var currentVel = p.rb.linearVelocity;
        var currentDir = currentVel.sqrMagnitude > 0.0001f ? currentVel.normalized : p.transform.forward;

        var maxRadians = Mathf.Deg2Rad * turnSpeedDegreesPerSecond * Time.deltaTime;
        var newDir = Vector3.RotateTowards(currentDir, desiredDir, maxRadians, 0f);
        p.rb.linearVelocity = newDir * p.Speed;
        return true;
    }

    private EnemyController FindNearestTarget(Vector3 origin)
    {
        var colliders = Physics.OverlapSphere(origin, homingRange);
        EnemyController best = null;
        var bestSqr = float.PositiveInfinity;

        foreach (var col in colliders)
        {
            var enemy = col.GetComponentInParent<EnemyController>();
            if (enemy == null)
            {
                continue;
            }

            var delta = enemy.transform.position - origin;
            delta.y = 0f;
            var sqr = delta.sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                best = enemy;
            }
        }

        return best;
    }
}
