using UnityEngine;

public class KnockbackProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private float force = 8f;

    public override bool OnHit(Projectile p, CharacterController target)
    {
        if (target == null || target.rb == null)
        {
            return false;
        }

        var dir = p.transform.forward;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f)
        {
            dir = (target.transform.position - p.transform.position);
            dir.y = 0f;
        }

        target.rb.AddForce(dir.normalized * force, ForceMode.Impulse);
        return false; // keep default damage + break
    }
}
