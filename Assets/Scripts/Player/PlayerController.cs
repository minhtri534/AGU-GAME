using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 10f;

    [Header("Stats")]
    public PlayerStatsData statsData;   // KÉO ASSET VÀO INSPECTOR

    private RuntimeCharacterStats stats;
    private IMovementInput input;
    private PlayerMotor motor;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = true;
        rb.useGravity = false;

        stats = new RuntimeCharacterStats(statsData);

        input = new KeyboardInput();
        motor = new PlayerMotor(rb, stats.Speed, rotationSpeed);
    }

    void FixedUpdate()
    {
        Vector3 moveDir = input.GetMovement();
        motor.Move(moveDir);
    }

    public RuntimeCharacterStats GetStats()
    {
        return stats;
    }
}
