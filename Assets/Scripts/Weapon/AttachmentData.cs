using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/AttachmentData")]
public class AttachmentData : ScriptableObject
{
    public string attachmentName = "New Attachment";
    public AttachmentType type = AttachmentType.Damage;
    public float value = 0f; // generic float value for most modifiers
    public int intValue = 0; // for integer changes like extra pellets or magazine
    public bool enable = true; // for toggles like auto enable

    public enum AttachmentType
    {
        Damage, // additive damage
        FireRateMultiplier, // multiply fire rate
        MagazineIncrease, // add magazinesize
        ReloadSpeedMultiplier, // multiply reload time
        ProjectileSpeedMultiplier, // multiply projectile speed
        SpreadReduction, // subtract degrees from spread
        PelletsIncrease, // add pellets
        AutoEnable // enable automatic fire
    }
}
