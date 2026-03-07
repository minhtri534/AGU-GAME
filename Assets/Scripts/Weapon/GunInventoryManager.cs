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
            // Toggles UI
            GunInventoryUI.enabled = !GunInventoryUI.enabled;
            // Get component
            if (GunInventoryUI.enabled)
            {
                // Disable gameplay input
                DisablePlayerInput(true);

                // Pick up component and remove the component just picked up from the ground
                if (SelectedObject != null)
                {
                    selectedComponent = SelectedObject.GetGunComponent();
                    Destroy(SelectedObject.gameObject);
                    SelectedObject = null;
                }

                // Update texture of UI when open inventory
                UpdateUI();
            }
            else
            {
                // Enable gameplay input
                DisablePlayerInput(false);
                if (selectedComponent != null)
                {
                    // get player position
                    var pos = GameObject.FindGameObjectWithTag("Player").transform.position;
                    // spawn new component world object
                    GunComponentWorldObjectInstancer.Spawn(selectedComponent, pos);
                    selectedComponent = null;
                }
            }
        }
        // Update the sprite of the selected component and make it follow the mouse
        if (GunInventoryUI.enabled)
        {
            var image = MouseSprite.GetComponent<Image>();
            if (selectedComponent != null)
            {
                MouseSprite.transform.position = Mouse.current.position.ReadValue();
                image.sprite = selectedComponent.Sprite;
                image.enabled = true;
            }
            else
            {
                image.enabled = false; // hide image if there is no component
            }

        }

    }

    public void OnComponentButtonPressed(int slot)
    {
        BehaviourModifierComponent component = null;
        if (selectedComponent != null)
        {
            if (selectedComponent.IsTypeComponent())
            {
                return;
            }
            else
            {
                component = (BehaviourModifierComponent)selectedComponent;
            }
        }

        // Swap components
        selectedComponent = SelectedInventory.SwapBehaviourModifierComponent(slot, component);
        // Update button ui
        UpdateUI();
    }

    public void OnTypeComponentButtonPressed()
    {
        TypeModifierComponent component = null;
        if (selectedComponent != null)
        {
            if (selectedComponent.IsTypeComponent())
            {
                component = (TypeModifierComponent)selectedComponent;
            }
            else
            {
                return;
            }
        }

        // Swap component
        selectedComponent = SelectedInventory.SwapTypeModifierComponent(component);
        // Update button ui
        UpdateUI();
    }

    private void UpdateUI()
    {
        Sprite s;
        Image image;
        for (int i = 0; i < 7; i++)
        {
            s = SelectedInventory.GetBehaviourModifierComponent(i)?.Sprite;
            image = Buttons[i].GetComponentsInChildren<Image>(true)[1];
            if (s != null)
            {
                image.enabled = true;
                image.sprite = s;
            }
            else
            {
                image.enabled = false; // hide image if there is no component
            }
        }
        s = SelectedInventory.GetTypeModifierComponent()?.Sprite;
        image = Buttons[7].GetComponentsInChildren<Image>(true)[1];
        if (s != null)
        {
            image.enabled = true;
            image.sprite = s;
        }
        else
        {
            image.enabled = false; // hide image if there is no component
        }
    }

    private void DisablePlayerInput(bool disabled)
    {
        if (disabled)
        {
            InputSystem.actions.FindAction("Player/Shoot").Disable();
            InputSystem.actions.FindAction("Player/Move").Disable();
            InputSystem.actions.FindAction("Player/Ability").Disable();
        }
        else
        {
            InputSystem.actions.FindAction("Player/Shoot").Enable();
            InputSystem.actions.FindAction("Player/Move").Enable();
            InputSystem.actions.FindAction("Player/Ability").Enable();
        }
    }
}