using System;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : MonoBehaviour
{
    public LootData Data;
    enum ItemType
    {
        None,
        Component,
        Rarity,
    }

    public void Start()
    {
        var renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        renderer.material.mainTexture = Data.Sprite.texture;
    }

    public void SpawnComponents()
    {
        LootDataItem temp = null;
        List<DictionaryItem<ItemType, string>> items = new();
        List<uint> itemWeights = new();
        List<BaseGunComponent> components = new();
        foreach (var loot in Data.Loot)
        {
            Debug.LogWarning(loot);
            // if the first item use previous data is true
            // then just use it anyway
            if (temp == null || !loot.UsePreviousDataItem)
            {
                temp = loot;
                itemWeights = new();
                items = new();

                // add all the weights into a single array to send to random
                // map each item-weight to their own array
                itemWeights.Add(temp.NotSpawningWeight);
                items.Add(new DictionaryItem<ItemType, string>(ItemType.None, ""));
                foreach (var r in temp.ComponentRarity)
                {
                    items.Add(new DictionaryItem<ItemType, string>(ItemType.Rarity, r.Key.ToString()));
                    itemWeights.Add(r.Value);
                }
                foreach (var c in temp.SpawnableComponents)
                {
                    items.Add(new DictionaryItem<ItemType, string>(ItemType.Component, c.Key));
                    itemWeights.Add(c.Value);
                }
            }

            // randomize an item (whether the component spawns, if its a specific component, or a random rarity)
            var chosenItem = items[RandomWeight.Random(itemWeights.ConvertAll<int>(a => (int)a).ToArray())];
            BaseGunComponent randomizedComponent = null;
            switch (chosenItem.Key)
            {
                case ItemType.None:
                    continue;
                case ItemType.Component:
                    randomizedComponent = GunComponentRepository.CreateGunComponent(chosenItem.Value);
                    break;
                case ItemType.Rarity:
                    randomizedComponent = GunComponentRepository.GetRandomGunComponentByRarity(Enum.Parse<Rarity>(chosenItem.Value));
                    break;
            }
            // add to a list to generate components after random process is complete
            components.Add(randomizedComponent);

            // TODO: 
            // get spawn coordinates depending on current location
            // and math out the location of each component
            // depending on the number of components
        }
        foreach (var c in components)
        {
            GunComponentWorldObjectInstancer.Spawn(c, transform.position + new Vector3(UnityEngine.Random.Range(-2, 2), 0, UnityEngine.Random.Range(-2, 2)));
            Debug.Log(c);
        }
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Chest opened");
        SpawnComponents();
    }
}
