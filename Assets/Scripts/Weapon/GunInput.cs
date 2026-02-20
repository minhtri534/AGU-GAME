using UnityEngine;
using UnityEngine.InputSystem;

public class GunInput : MonoBehaviour
{
    private InputAction shootAction;

    private void Start()
    {
        shootAction = InputSystem.actions.FindAction("Player/Shoot");
    }
    public GunInputState GetInput()
    {

        if (shootAction.WasPressedThisFrame())
        {
            return GunInputState.JustPressed;
        }
        if (shootAction.WasReleasedThisFrame())
        {
            return GunInputState.JustReleased;
        }
        if (shootAction.IsPressed())
        {
            return GunInputState.Held;
        }
        return GunInputState.None;
    }
}

public enum GunInputState
{
    None,
    JustPressed,
    Held,
    JustReleased,
}
