using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GunInventoryManager : MonoBehaviour
{
    public static GunComponentWorldObject SelectedObject;
    public static GunComponentInventory SelectedInventory;
    public Canvas GunInventoryUI;
    public Image MouseSprite;
    public Button[] Buttons = new Button[8];
    public TextMeshProUGUI EquippedText;
    public TextMeshProUGUI HoldingText;
    private BaseGunComponent selectedComponent;
    private InputAction interactAction;
    private InputAction inventoryAction;
    void Start()
    {
        interactAction = InputSystem.actions.FindAction("Player/Interact");
        inventoryAction = InputSystem.actions.FindAction("Player/Inventory");

        // Add listeners to buttons and such
        for (int i = 0; i < 7; i++)
        {
            int amogus = i; // stupid code thing needs stupid fix
            Buttons[i].onClick.AddListener(() => OnComponentButtonPressed(amogus));
            Buttons[i].gameObject.GetComponent<PointerEvent>().onPointerEnter.AddListener(() => OnButtonEntered(amogus));
            Buttons[i].gameObject.GetComponent<PointerEvent>().onPointerExit.AddListener(() => OnButtonExited(amogus));
        }
        Buttons[7].onClick.AddListener(() => OnTypeComponentButtonPressed());
        Buttons[7].gameObject.GetComponent<PointerEvent>().onPointerEnter.AddListener(() => OnButtonEntered(7));
        Buttons[7].gameObject.GetComponent<PointerEvent>().onPointerExit.AddListener(() => OnButtonExited(7));

        // TODO: Find and add the gun inventory of the player automatically
    }

    void Update()
    {
        if (inventoryAction.WasPressedThisFrame())
        {
            ToggleInventory();
        }
        else if (interactAction.WasPressedThisFrame())
        {
            // Pick up component and remove the component just picked up from the ground
            if (SelectedObject != null)
            {
                selectedComponent = SelectedObject.GetGunComponent();
                Destroy(SelectedObject.gameObject);
                SelectedObject = null;
                ToggleInventory();
            }
        }
        // Update the sprite of the selected component and make it follow the mouse
        if (GunInventoryUI.enabled)
        {
            var image = MouseSprite.GetComponent<Image>();
            HoldingText.text = ParseComponentData(selectedComponent);
            if (selectedComponent != null)
            {
                MouseSprite.transform.position = Mouse.current.position.ReadValue();
                image.sprite = selectedComponent.Data.Sprite;
                image.enabled = true;
            }
            else
            {
                image.enabled = false; // hide image if there is no component
            }
        }
    }

    private void ToggleInventory()
    {
        // Toggles UI
        GunInventoryUI.enabled = !GunInventoryUI.enabled;
        // Get component
        if (GunInventoryUI.enabled)
        {
            // Disable gameplay input
            DisablePlayerInput(true);

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

    // Update whenever a component is equipped/unequipped
    private void UpdateUI()
    {
        Sprite s;
        Image image;
        for (int i = 0; i < 7; i++)
        {
            s = SelectedInventory.GetBehaviourModifierComponent(i)?.Data.Sprite;
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
        s = SelectedInventory.GetTypeModifierComponent()?.Data.Sprite;
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
            InputSystem.actions.FindAction("Player/Interact").Disable();
        }
        else
        {
            InputSystem.actions.FindAction("Player/Shoot").Enable();
            InputSystem.actions.FindAction("Player/Move").Enable();
            InputSystem.actions.FindAction("Player/Ability").Enable();
            InputSystem.actions.FindAction("Player/Interact").Enable();
        }
    }

    private string ParseComponentData(BaseGunComponent component)
    {
        string text = "";
        var data = component?.Data;
        if (data != null)
        {
            text = @$"{data.ComponentName}
    {data.Rarity}\n
{data.Description}
                ";
        }
        return text;
    }

    private void OnButtonEntered(int slot)
    {
        BaseGunComponent component;
        if (slot != 7)
        {
            component = SelectedInventory.GetBehaviourModifierComponent(slot);
        }
        else
        {
            component = SelectedInventory.GetTypeModifierComponent();
        }
        EquippedText.text = ParseComponentData(component);
    }

    private void OnButtonExited(int slot)
    {
        EquippedText.text = "";
    }
}