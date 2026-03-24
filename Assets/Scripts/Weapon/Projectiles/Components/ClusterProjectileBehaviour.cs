using UnityEngine;

public class ClusterProjectileBehaviour : BaseProjectileComponent
{
    [SerializeField] private int splitCount = 6;
    [SerializeField] private float childDamageMultiplier = 0.5f;
    [SerializeField] private float childSpeedMultiplier = 0.9f;
    [SerializeField] private int generationsRemaining = 1;

    public override bool OnBreak(Projectile p)
    {
        if (generationsRemaining <= 0)
        {
            return false;
        }

        var count = Mathf.Max(2, splitCount);
        for (int i = 0; i < count; i++)
        {
            var child = Object.Instantiate(p);

            child.transform.position = p.transform.position + (child.transform.forward * 0.25f);
            child.transform.rotation = Quaternion.AngleAxis((360f / count) * i, Vector3.up) * p.transform.rotation;

            child.Stats.SetDamage(p.Stats.GetDamage() * childDamageMultiplier);
            child.Stats.SetProjectileSpeed(p.Stats.GetProjectileSpeed() * childSpeedMultiplier);

            var cluster = child.GetComponent<ClusterProjectileBehaviour>();
            if (cluster != null)
            {
                cluster.generationsRemaining = generationsRemaining - 1;
            }
        }

        return false;
    }
}
