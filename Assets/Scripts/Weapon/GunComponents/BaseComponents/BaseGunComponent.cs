using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Base gun component class which contains metadata and gamplay data
/// </summary>
public abstract class BaseGunComponent
{
    protected bool isTypeComponent = false;
    public GunComponentData Data;


    public bool IsTypeComponent()
    {
        return isTypeComponent;
    }
}
