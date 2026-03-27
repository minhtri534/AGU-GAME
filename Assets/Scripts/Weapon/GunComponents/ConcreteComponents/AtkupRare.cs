using Unity;

public class AtkUpRare : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 1.5f);
    }

}