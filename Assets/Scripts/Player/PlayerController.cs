using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private IMovementInput input;
    private PlayerMotor motor;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = true;
        rb.useGravity = false;

        input = new KeyboardInput();
        motor = new PlayerMotor(rb, moveSpeed, rotationSpeed);
    }

    void FixedUpdate()
    {
        Vector3 moveDir = input.GetMovement();
        motor.Move(moveDir);
    }
}
