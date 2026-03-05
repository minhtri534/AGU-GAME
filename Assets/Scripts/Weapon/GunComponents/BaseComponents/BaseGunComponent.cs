using UnityEngine;

/// <summary>
/// Base gun component class which contains metadata and gamplay data
/// </summary>
public abstract class BaseGunComponent
{
    // Metadata
    public virtual string ComponentId { get {return "DefaultComponent";}}
    public virtual string Description { get {return "DefaultComponent";}}
    protected virtual string SpritePath { get {return "Sprites/tree";}} // placeholder sprite for now, update this to an actual default sprite
    public readonly Sprite Sprite;
    protected bool isTypeComponent = false;
    // Gameplay data
    public virtual Rarity Rarity { get {return Rarity.Common;}}
    public BaseGunComponent()
    {
        Sprite = Resources.Load<Sprite>(SpritePath);
    }

    public bool IsTypeComponent()
    {
        return isTypeComponent;
    }
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary
}