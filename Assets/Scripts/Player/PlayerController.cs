using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : CharacterController
{
    [Header("Stats")]
    public PlayerStatsData statsData;
    private IMovementInput input;
    private PlayerMotor motor;

    private IPlayerSkill skill;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;   

        stats = new RuntimeCharacterStats(statsData);
        stats.IsDead.AddListener(Die);
        stats.IsHurt.AddListener(TakeDamageAnimation);

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

    public void Die()
    {
        Destroy(gameObject);
    }
}
