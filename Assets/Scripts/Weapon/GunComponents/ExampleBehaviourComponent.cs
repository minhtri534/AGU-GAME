using UnityEngine;

/// <summary>
/// Example class implementation of a gun component
/// </summary>
public class ExampleBehaviourComponent : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        // Doubles the damage of the bullet
        stats.SetDamage(stats.GetDamage() * 2);
        // Increases the number of projectiles
        stats.SetNumberOfProjectiles(stats.GetNumberOfProjectiles() + 2);
        // Increases inaccuracy
        stats.SetInaccuary(stats.GetInaccuracy() * 2);
        // Change shot type
        stats.ShotType = ShotType.Multishot;
        stats.SetProjectileLifeTime(100);
        stats.SetReloadTime(0.2f);
        //stats.GetExtraStat("a"); // TEST CODE
    }

    public override void AddComponentsToProjectile(Projectile p)
    {
        p.addProjectileComponent(p.gameObject.AddComponent<ExampleProjectileBehaviour>());
    }
}