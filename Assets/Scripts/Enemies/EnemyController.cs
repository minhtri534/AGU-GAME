using UnityEngine;
using Photon.Pun;

public class EnemyController : CharacterController
{
    [Header("Stats")]
    public EnemyStatsData statsData;

    [Header("AI")]
    public EnemyStateMachine StateMachine;

    [HideInInspector] public Transform player;
    public System.Action<EnemyController> OnEnemyDeath;
    private PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    void Start()
    {
        // Khởi tạo stats
        stats = new RuntimeCharacterStats(statsData);

        // Đăng ký sự kiện
        stats.IsDead.AddListener(Die);
        stats.IsHurt.AddListener(TakeDamageAnimation);

        // Fix delegate và check KeyNotFoundException cho StateMachine
        stats.IsHurt.AddListener(delegate {
            if (StateMachine != null && StateMachine.Parameters.ContainsKey("TakeDamageTrigger"))
            {
                StateMachine.SetParameterTrigger("TakeDamageTrigger");
            }
            else if (StateMachine != null)
            {
                Debug.LogWarning($"StateMachine của {gameObject.name} thiếu Parameter: TakeDamageTrigger");
            }
        });

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (StateMachine != null)
        {
            StateMachine.Enemy = this;
            StateMachine.Start();
        }
    }

    void FixedUpdate()
    {
        // Chỉ Master Client hoặc người sở hữu mới chạy logic AI
        if (!photonView.IsMine && !PhotonNetwork.IsMasterClient) return;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            return;
        }

        if (StateMachine != null)
        {
            // Kiểm tra key trước khi set để tránh crash
            if (StateMachine.Parameters.ContainsKey("DistanceToPlayer"))
            {
                StateMachine.SetParameterFloat("DistanceToPlayer", (player.position - transform.position).magnitude);
            }
            StateMachine.Update();
        }
    }

    public void TakeDamage(float damage)
    {
        // Log tại máy người bắn (Local)
        Debug.Log($"[Local] Bắn trúng {gameObject.name}, gửi RPC gây {damage} dmg");
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (stats != null)
        {
            // Ép kiểu về RuntimeCharacterStats để truy cập CurrentHP
            RuntimeCharacterStats runtimeStats = stats as RuntimeCharacterStats;
            runtimeStats.TakeDamage(damage);

            // Log tại tất cả các máy (Fix lỗi compile tại đây)
            Debug.Log($"[Network] {gameObject.name} nhận {damage} dmg. Máu còn lại: {runtimeStats.CurrentHP}");
        }
    }

    public void Die()
    {
        if (photonView.IsMine)
        {
            Debug.Log($"{gameObject.name} đã chết. Đang xóa...");
            OnEnemyDeath?.Invoke(this);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}