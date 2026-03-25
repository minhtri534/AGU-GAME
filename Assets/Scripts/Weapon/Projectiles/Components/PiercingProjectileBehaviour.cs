using UnityEngine;

public class PiercingProjectileBehaviour : BaseProjectileComponent
{
    private int numberOfHits = 0;
    public override bool OnHit(Projectile p, CharacterController target)
    {
        if (target != null && target.stats != null)
        {
            target.stats.TakeDamage(p.Damage);
        }
        numberOfHits += 1;
        if (numberOfHits > p.GunStats.GetExtraStat("Pierce"))
        {
            p.QueueOnBreak();
        }
        return true;
    }
}