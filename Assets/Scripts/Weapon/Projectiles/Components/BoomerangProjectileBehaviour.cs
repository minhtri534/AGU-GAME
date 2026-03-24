using UnityEngine;

public class BoomerangProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float outboundTime = 0.6f;
    [SerializeField] private float returnTime = 0.8f;

    private float timer;
    private Vector3 initialDir;

    public override bool OnShot(Projectile p)
    {
        timer = 0f;
        initialDir = p.transform.forward;
        initialDir.y = 0f;
        if (initialDir.sqrMagnitude < 0.0001f)
        {
            initialDir = Vector3.forward;
        }
        initialDir.Normalize();
        return false;
    }

    public override bool OnTraveling(Projectile p)
    {
        timer += Time.deltaTime;

        Vector3 dir;
        if (timer <= outboundTime)
        {
            dir = initialDir;
        }
        else if (timer <= outboundTime + returnTime)
        {
            dir = -initialDir;
        }
        else
        {
            // after returning for a while, just keep going forward again
            dir = initialDir;
        }

        p.rb.linearVelocity = dir * p.Stats.GetProjectileSpeed();
        return true;
    }
}
