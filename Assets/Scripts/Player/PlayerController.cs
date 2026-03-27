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

        CheckStats();

        stats.IsDead.AddListener(Die);
        stats.IsHurt.AddListener(TakeDamageAnimation);

        input = new KeyboardInput();
        motor = new PlayerMotor(rb, stats.Speed);

        skill = GetComponent<IPlayerSkill>();
    }

    private void CheckStats()
    {
        if (stats == null && statsData != null)
        {
            stats = new RuntimeCharacterStats(statsData);
        }
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
        if (input != null && motor != null)
        {
            Vector3 moveDir = input.GetMovement();
            motor.Move(moveDir);
        }
    }

    public RuntimeCharacterStats GetStats()
    {
        CheckStats(); 
        return stats;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}