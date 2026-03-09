using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class GunComponentRepository
{
    private static readonly Dictionary<string, GunComponentData> repository = new();

    static GunComponentRepository()
    {
        LoadRepository();
    }

    // Automatically add components to the list when added to the folder
    // All components must be manually registered in a GunComponentData asset
    private static void LoadRepository()
    {
        var data = Resources.LoadAll<GunComponentData>("GunComponentData");
        foreach (var d in data)
        {
            repository.Add(d.ComponentClass, d);
        }
    }

    public static GunComponentData GetGunComponentData(string component)
    {
        return repository[component];
    }

    public static BaseGunComponent CreateGunComponent(string component)
    {
        var t = Type.GetType(component);
        if (t == null)
        {
            Debug.LogError(component + " does not exist");
            return null;
        }
        var c = (BaseGunComponent)Activator.CreateInstance(t);
        // Inject the data from the repo back into the object
        c.Data = GetGunComponentData(component);
        return c;
    }

    public static T CreateGunComponent<T>() where T : BaseGunComponent
    {
        var component = (T)Activator.CreateInstance(typeof(T));
        // Inject the data from the repo back into the object
        component.Data = repository[typeof(T).Name];
        return component;
    }

    public static BaseGunComponent GetRandomGunComponent(string[] exclusion = null)
    {
        string randomID;
        if (exclusion != null)
        {
            // Clone the dictionary into an array with all the keys in the dictionary except for the entries in exclusion
            var tempRepo = repository.Keys.Where(a => !exclusion.Contains(a)).ToArray();

            randomID = tempRepo[UnityEngine.Random.Range(0, tempRepo.Length)];
        }
        else
        {
            randomID = repository.Keys.ToArray()[UnityEngine.Random.Range(0, repository.Count)];
        }
        return CreateGunComponent(randomID);
    }

    public static BaseGunComponent GetRandomGunComponentByRarity(Rarity rarity, string[] exclusion = null)
    {
        string randomID;
        if (exclusion != null)
        {
            var tempRepo = repository.Values.Where(a => a.Rarity == rarity && !exclusion.Contains(a.ComponentClass)).ToArray();
            randomID = tempRepo[UnityEngine.Random.Range(0, tempRepo.Length)].ComponentClass;
        }
        else
        {
            randomID = repository.Values.Where(a => a.Rarity == rarity).ToArray()[UnityEngine.Random.Range(0, repository.Count)].ComponentClass;
        }

        return CreateGunComponent(randomID);
    }
}

