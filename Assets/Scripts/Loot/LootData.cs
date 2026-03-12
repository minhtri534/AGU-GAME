using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Loot Data")]
public class LootData : ScriptableObject
{
    public Sprite Sprite;
    /// <summary>
    /// The loot statistics for each item in a chest
    /// </summary>
    /// <remarks>
    /// If one list item is left empty, the data will be copied from the
    /// list item before it
    /// </remarks>
    [Tooltip("If one list item is left empty, the data will be copied from the list item before it")]
    public List<LootDataItem> Loot;
}
