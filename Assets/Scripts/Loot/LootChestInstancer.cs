using UnityEngine;

public static class LootChestInstancer
{
    private static readonly GameObject prefab = Resources.Load<GameObject>("Prefabs/LootChest");
    public static void Spawn(LootData data, Vector3 pos)
    {
        var obj = Object.Instantiate(prefab, pos, Quaternion.identity);
        var chest = obj.GetComponent<LootChest>();
        chest.Data = data;
    } 
}