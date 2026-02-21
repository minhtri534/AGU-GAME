using System.ComponentModel;

/// <summary>
/// Handles the component inventory of the gun
/// </summary>
public class GunComponentInventory
{
    public int InventorySize = 7;
    private readonly BehaviourModifierComponent[] behaviourModifierComponents;
    private TypeModifierComponent typeModifierComponent;
    // access to the guns stats for updating it
    private GunStats stats;
    
    public GunComponentInventory(GunStats s)
    {
        stats = s;
        behaviourModifierComponents = new BehaviourModifierComponent[InventorySize];
    }

    /// <summary>
    /// Swaps a slot with a new component and returns the previous component
    /// </summary>
    /// <remarks>
    /// Leave the component param empty to remove component.
    /// If the slot is empty then new component is simply added
    /// </remarks>
    /// <param name="slot"></param>
    /// <param name="component"></param>
    /// <returns>Returns old component or null if slot is empty</returns>
    public BehaviourModifierComponent SwapBehaviourModifierComponent(int slot, BehaviourModifierComponent component = null)
    {
        BehaviourModifierComponent oldComponent = behaviourModifierComponents[slot];
        behaviourModifierComponents[slot] = component;
        UpdateStats();
        return oldComponent;
    }
    /// <summary>
    /// Swaps with a new component and returns the previous component
    /// </summary>
    /// <remarks>
    /// Leave the component param empty to remove component.
    /// If the slot is empty then new component is simply added
    /// </remarks>
    /// <param name="component"></param>
    /// <returns>Returns old component or null if slot is empty</returns>
    public TypeModifierComponent SwapTypeModifierComponent(TypeModifierComponent component = null)
    {
        TypeModifierComponent oldComponent = typeModifierComponent;
        typeModifierComponent = component;
        UpdateStats();
        return oldComponent;
    }
    /// <summary>
    /// Get an item from component array at specified slot
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public BehaviourModifierComponent GetBehaviourModifierComponent(int slot)
    {
        return behaviourModifierComponents[slot];
    }
    public TypeModifierComponent GetTypeModifierComponent()
    {
        return typeModifierComponent;
    }
    private void UpdateStats()
    {
        // reset stats to base stats
        stats.ResetStats();
        // apply type component stats first
        typeModifierComponent?.ModifyStats(stats);
        // update stats for all components in inventory
        foreach (BehaviourModifierComponent component in behaviourModifierComponents)
        {
            component?.ModifyStats(stats);
        }
    }
}