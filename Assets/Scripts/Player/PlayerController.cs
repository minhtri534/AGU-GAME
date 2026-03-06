using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public PlayerStatsData statsData;

    private RuntimeCharacterStats stats;
    private IMovementInput input;
    private PlayerMotor motor;
    private Rigidbody rb;

    private IPlayerSkill skill;  

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;   

        stats = new RuntimeCharacterStats(statsData);

        input = new KeyboardInput();
        motor = new PlayerMotor(rb, stats.Speed);

        skill = GetComponent<IPlayerSkill>(); 
    }

    void Update()
    {
        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            skill?.Activate();
        }
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
