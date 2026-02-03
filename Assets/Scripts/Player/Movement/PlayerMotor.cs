using UnityEngine;

public class PlayerMotor
{
    private Rigidbody rb;
    private float moveSpeed;
    private float rotationSpeed;

    public PlayerMotor(Rigidbody rb, float moveSpeed, float rotationSpeed)
    {
        this.rb = rb;
        this.moveSpeed = moveSpeed;
        this.rotationSpeed = rotationSpeed;
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Vector3 nextPosition =rb.position + direction * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(nextPosition);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}
