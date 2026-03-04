using UnityEngine;

/// <summary>
/// Base gun component class which contains metadata and gamplay data
/// </summary>
public abstract class BaseGunComponent
{
    // Metadata
    public string ComponentId = "DefaultComponent";
    public string Description = "DefaultComponent";
    public readonly Texture Sprite = Resources.Load<Texture>("Sprites/tree"); // placeholder sprite for now, update this to an actual default sprite
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