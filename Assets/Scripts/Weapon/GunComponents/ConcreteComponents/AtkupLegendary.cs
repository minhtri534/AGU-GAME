using Unity;

public class AtkupLegendary : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 2.0f);
    }

}