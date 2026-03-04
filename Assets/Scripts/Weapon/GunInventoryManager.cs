using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class GunInventoryManager : MonoBehaviour
{
    public static GunComponentWorldObject SelectedObject;
    public static GunComponentInventory SelectedInventory;
    public Canvas GunInventoryUI;
    public GameObject MouseSprite;
    public GameObject[] Buttons = new GameObject[8];
    private BaseGunComponent selectedComponent;
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

            // Toggles UI
            GunInventoryUI.enabled = !GunInventoryUI.enabled;
            // Get component
            if (GunInventoryUI.enabled)
            {
                // Pick up component and remove the component just picked up form the ground
                selectedComponent = (BaseGunComponent)SelectedObject.GetGunComponent();
                Destroy(SelectedObject.gameObject);
                SelectedObject = null;
                // Update texture of UI
                for (int i = 0;i < 7;i++)
                {
                    Buttons[i].GetComponentInChildren<Image>().material.mainTexture = SelectedInventory.GetBehaviourModifierComponent(i).Sprite;
                }
                Buttons[7].GetComponentInChildren<Image>().material.mainTexture = SelectedInventory.GetTypeModifierComponent().Sprite;
            } else
            {
                if (selectedComponent != null)
                {
                    // get player position
                    var pos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    // spawn new component world object
                    GunComponentWorldObjectInstancer.Spawn(selectedComponent, pos);
                }
            }
        }
        if (GunInventoryUI.enabled)
        {
            MouseSprite.transform.position = Mouse.current.position.ReadValue();
            MouseSprite.GetComponent<Image>().material.mainTexture = selectedComponent.Sprite;
        }
        
    }

    public void OnComponentButtonPressed(int slot)
    {
        BehaviourModifierComponent component = (BehaviourModifierComponent)selectedComponent;

        selectedComponent = SelectedInventory.SwapBehaviourModifierComponent(slot, component);
        Debug.Log(selectedComponent);
    }

    public void OnTypeComponentButtonPressed()
    {
        TypeModifierComponent component = null;
        if (SelectedObject != null)
        {  
            if (SelectedObject.IsTypeComponent())
            {
                component = (TypeModifierComponent)SelectedObject.GetGunComponent();
            } else
            {
                return;
            }
        }
        selectedComponent = SelectedInventory.SwapTypeModifierComponent(component);
    }
}