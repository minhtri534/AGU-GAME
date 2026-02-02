using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Rigidbody rb;
    private Vector3 movementInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal = -1f;
        else if (Keyboard.current.dKey.isPressed) horizontal = 1f;

        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        else if (Keyboard.current.sKey.isPressed) vertical = -1f;

        movementInput = new Vector3(-horizontal, 0f, -vertical).normalized;
    }

    void FixedUpdate()
    {
        if (movementInput == Vector3.zero)
            return;

        Vector3 nextPosition =
            rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(nextPosition);

        Quaternion targetRotation = Quaternion.LookRotation(movementInput);
        rb.MoveRotation(
            Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime)
        );
    }
}
