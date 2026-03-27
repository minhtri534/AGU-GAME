using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun; 

public class PlayerController : CharacterController
{
    [Header("Stats")]
    public PlayerStatsData statsData;
    private IMovementInput input;
    private PlayerMotor motor;
    private IPlayerSkill skill;

    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        CheckStats();

        stats.IsDead.AddListener(Die);
        stats.IsHurt.AddListener(TakeDamageAnimation);

        if (photonView.IsMine)
        {
            input = new KeyboardInput();
            motor = new PlayerMotor(rb, stats.Speed);
        }

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
        if (!photonView.IsMine) return;

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            skill?.Activate();
        }
    }

    void FixedUpdate()
    {
        if (photonView.IsMine && input != null && motor != null)
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
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}