using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInput : IMovementInput
{
    private InputAction moveAction;

    public KeyboardInput()
    {
        moveAction = InputSystem.actions.FindAction("Player/move");
    }
    public Vector3 GetMovement()
    {
        var movement = moveAction.ReadValue<Vector2>();
        return new Vector3(movement.x, 0, movement.y);
    }
}
