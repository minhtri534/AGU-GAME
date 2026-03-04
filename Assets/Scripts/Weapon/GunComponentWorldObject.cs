using System;
using Unity.Mathematics;
using UnityEngine;

public class GunComponentWorldObject : MonoBehaviour
{
    private bool isTypeComponent;
    private BaseGunComponent gunComponent;

    public bool IsTypeComponent()
    {
        return isTypeComponent;
    }

    public void SetGunComponent(BaseGunComponent c)
    {
        gunComponent = c;
        if (c.GetType() == typeof(TypeModifierComponent))
        {
            isTypeComponent = true;
        }
        else
        {
            isTypeComponent = false;
        }
    }

    public BaseGunComponent GetGunComponent()
    {
        return gunComponent;
    }
    public void Start()
    {
        // get the gameobject
        // use the sprite path from the component to load the new sprite
        // change the texture of the material to the new sprite
        var renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        renderer.material.mainTexture = gunComponent.Sprite;
    }

    // Create function to detect when the player is in the trigger
    // and pressing interact
    // After pressing interact, open the inventory ui (and manager class)
    // to let player equip component
    // Remember to destroy self after equipping
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GunInventoryManager.SelectedObject = this;
            Debug.Log("Component detected!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GunInventoryManager.SelectedObject == this)
            {
                GunInventoryManager.SelectedObject = null;
                Debug.Log("Component undetected!");
            }
        }
    }
}