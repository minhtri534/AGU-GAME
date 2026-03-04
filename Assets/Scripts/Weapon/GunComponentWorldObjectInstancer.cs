using UnityEngine;

public static class GunComponentWorldObjectInstancer
{
    private static readonly GameObject prefab = Resources.Load<GameObject>("Prefabs/ComponentObject");
    public static void Spawn(BaseGunComponent c, Vector3 pos)
    {
        var obj = Object.Instantiate(prefab, pos, Quaternion.identity);
        var component = obj.GetComponent<GunComponentWorldObject>();
        component.SetGunComponent(c);
    }
}