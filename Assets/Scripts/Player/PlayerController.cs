using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStatsData statsData;   // K�O ASSET V�O INSPECTOR

    private RuntimeCharacterStats stats;
    private IMovementInput input;
    private PlayerMotor motor;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        stats = new RuntimeCharacterStats(statsData);

        input = new KeyboardInput();
        motor = new PlayerMotor(rb, stats.Speed);
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
