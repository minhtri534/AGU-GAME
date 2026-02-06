using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInput : IMovementInput
{
    public Vector3 GetMovement()
    {
        if (Keyboard.current == null)
            return Vector3.zero;

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        else if (Keyboard.current.dKey.isPressed) horizontal = 1f;

        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        else if (Keyboard.current.sKey.isPressed) vertical = -1f;

        return new Vector3(horizontal, 0f, vertical).normalized;
    }
}
