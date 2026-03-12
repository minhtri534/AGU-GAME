using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LootDataItem
{
    [Tooltip("Set to true to use previous loot data item in list\nWill not work if this is the first item in the list")]
    public bool UsePreviousDataItem = false;
    [Tooltip("If set higher than 0 the item has a chance to not be spawned")]
    public uint NotSpawningWeight = 0;
    [Tooltip("Components that can be spawned and the weight of each")]
    public List<DictionaryItem<string, uint>> SpawnableComponents;
    [Tooltip("The weight for a random component of a specific rarity to be chosen instead")]
    public List<DictionaryItem<Rarity, uint>> ComponentRarity = new()
    {
        {new DictionaryItem<Rarity, uint>(Rarity.Common, 0)},
        {new DictionaryItem<Rarity, uint>(Rarity.Uncommon, 0)},
        {new DictionaryItem<Rarity, uint>(Rarity.Rare, 0)},
        {new DictionaryItem<Rarity, uint>(Rarity.Epic, 0)},
        {new DictionaryItem<Rarity, uint>(Rarity.Legendary, 0)},
    };
    [Tooltip("Components that cannot be spawned\nMeant to be used with random rarity component")]
    public List<string> ExcludedComponents;
}
