using Unity;

public class AtkupUncommon : BehaviourModifierComponent
{
    public override void ModifyStats(GunStats stats)
    {
        stats.SetDamage(stats.GetDamage() * 1.25f);
    }

}