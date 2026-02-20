public abstract class BaseGunComponent
{
    // Metadata
    public string ComponentId = "DefaultComponent";
    public string Description = "DefaultComponent";
    public string SpritePath = "";
    // Gameplay data
    public Rarity rarity = Rarity.Common;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}