using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunInventoryManager : MonoBehaviour
{
    public static GunComponentWorldObject SelectedObject;
    public static GunComponentInventory SelectedInventory;
    private InputAction interactAction;
    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Player/Interact");
        // TODO: Find and add the gun inventory of the player automatically
    }

    void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            // TODO: Open UI of the inventory
            // When the player click on a slot, check if the type of component is correct for that slot
            // If yes, swap the component in the slot with the SelectedObject
            // Add a "Drop Component" UI as well to simply drop the component to the ground
            // When dropped, instantiate new component world object and attach the gun component to it
            BehaviourModifierComponent component = null;
            if (SelectedObject != null)
            {
                component = (BehaviourModifierComponent)SelectedObject.GetGunComponent();
            }
            SelectedInventory.SwapBehaviourModifierComponent(0, component);
        }
    }
}