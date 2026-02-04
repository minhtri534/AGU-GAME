using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public EnemyStatsData statsData;

    [Header("AI")]
    public float stoppingDistance = 1.5f;
    public Vector2 chaseTimeRange;
    public Vector2 orbitTimeRange;
    public Vector2 retreatTimeRange;

    [HideInInspector] public Transform player;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public float stateTimer;
    [HideInInspector] public int orbitDir;
    [HideInInspector] public RuntimeCharacterStats stats;

    private IEnemyState currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    void Start()
    {
        stats = new RuntimeCharacterStats(statsData);
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

        if (r < 0.4f)
            ChangeState(new EnemyChaseState());
        else if (r < 0.75f)
            ChangeState(new EnemyOrbitState());
        else
            ChangeState(new EnemyRetreatState());
    }
}
