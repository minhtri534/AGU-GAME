using Unity;

public class AtkUpEpic : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 1.75f);
    }

}