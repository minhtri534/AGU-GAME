using UnityEngine;

[CreateAssetMenu(menuName = "Game/Gun Component Data")]
public class GunComponentData : ScriptableObject
{
    public string ComponentClass = "BaseGunComponent"; // the class name of which this data belongs to
    public string ComponentName = "DefaultComponent";
    public string Description = "DefaultComponent";
    public Sprite Sprite;
    public Rarity Rarity = Rarity.Common;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}