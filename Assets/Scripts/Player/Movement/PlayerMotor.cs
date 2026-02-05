using UnityEngine;

public class PlayerMotor
{
    private Rigidbody rb;
    private float accelleration = 400;

    public PlayerMotor(Rigidbody rb, float moveSpeed)
    {
        this.rb = rb;
        rb.maxLinearVelocity = moveSpeed;
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, Vector3.zero, accelleration * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity += accelleration * Time.fixedDeltaTime * direction;
        }
    }
}
