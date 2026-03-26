using UnityEngine;

public class EnemyController : CharacterController
{
    [Header("Stats")]
    public EnemyStatsData statsData;

    [Header("AI")]
    public float stoppingDistance = 1.5f;
    public Vector2 chaseTimeRange;
    public Vector2 orbitTimeRange;
    public Vector2 retreatTimeRange;

    [HideInInspector] public Transform player;
    [HideInInspector] public float stateTimer;
    [HideInInspector] public int orbitDir;

    private IEnemyState currentState;
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
        // TODO: replace tag with specific individual players instead for multiplayer
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        ChangeState(new EnemyChaseState());
    }

    void FixedUpdate()
    {
        if (player == null) return;

        stateTimer -= Time.fixedDeltaTime;
        currentState.Update(this);

        if (stateTimer <= 0f)
            PickRandomState();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    void PickRandomState()
    {
        float r = Random.value;

        if (r < 0.2f)
            ChangeState(new EnemyChaseState());
        else if (r < 0.55f)
            ChangeState(new EnemyOrbitState());
        else if (r < 0.7f)
            ChangeState(new EnemyRetreatState());
        else
        {
            ChangeState(ScriptableObject.CreateInstance<EnemyAttackState>());
        }
    }

    public void Die()
    {
        OnEnemyDeath?.Invoke(this); 
        Destroy(gameObject);       
    }
}
