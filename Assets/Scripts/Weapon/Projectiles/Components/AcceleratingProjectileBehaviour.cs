using UnityEngine;

public class AcceleratingProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float accelerationPerSecond = 6f;
    [SerializeField] private float maxSpeedMultiplier = 2.5f;

    private float baseSpeed;

    public override bool OnShot(Projectile p)
    {
        baseSpeed = Mathf.Max(0.1f, p.Stats.GetProjectileSpeed());
        return false;
    }

    public override bool OnTraveling(Projectile p)
    {
        var maxSpeed = baseSpeed * Mathf.Max(1f, maxSpeedMultiplier);
        p.Stats.SetProjectileSpeed(Mathf.Min(maxSpeed, p.Stats.GetProjectileSpeed() + accelerationPerSecond * Time.deltaTime));
        p.rb.linearVelocity = p.transform.forward * p.Stats.GetProjectileSpeed();
        return true;
    }
}
