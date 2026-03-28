using UnityEngine;

public class EnemyController : CharacterController
{
    [Header("Stats")]
    public EnemyStatsData statsData;

    [Header("AI")]
    public EnemyStateMachine StateMachine;

    [HideInInspector] public Transform player;

    public System.Action<EnemyController> OnEnemyDeath;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
    }

    void Start()
    {
        stats = new RuntimeCharacterStats(statsData);
        stats.IsDead.AddListener(Die);
        stats.IsHurt.AddListener(TakeDamageAnimation);
        stats.IsHurt.AddListener(delegate{StateMachine.SetParameterTrigger("TakeDamageTrigger");});
        // TODO: replace tag with specific individual players instead for multiplayer
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        StateMachine.Enemy = this;
        StateMachine.Start();
    }

    void FixedUpdate()
    {
        if (player == null) return;
        StateMachine.SetParameterFloat("DistanceToPlayer", (player.transform.position - transform.position).magnitude);
        StateMachine.Update();
    }

    public void Die()
    {
        OnEnemyDeath?.Invoke(this); 
        Destroy(gameObject);       
    }
}
