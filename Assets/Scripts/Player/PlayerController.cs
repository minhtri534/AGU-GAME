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
            CameraFollow cam = FindObjectOfType<CameraFollow>();
            if (cam != null)
            {
                cam.SetTarget(this.transform);
            }
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
            photonView.RPC("RPC_ActivateSkill", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void RPC_ActivateSkill()
    {
        skill?.Activate();
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (input != null && motor != null)
            {
                Vector3 moveDir = input.GetMovement();
                motor.Move(moveDir);
            }

            if (RoomManager.instance != null)
            {
                RoomManager.instance.UpdateCurrentRoomByWorldPos(transform.position);
            }
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