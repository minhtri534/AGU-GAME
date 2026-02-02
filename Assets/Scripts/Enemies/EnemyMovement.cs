using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 1.5f;

    [Header("Behavior")]
    public float orbitSpeed = 2f;

    [Header("Retreat")]
    public float retreatSpeed = 2f;

    [Header("Timers")]
    public Vector2 chaseTimeRange = new Vector2(1.5f, 3f);
    public Vector2 orbitTimeRange = new Vector2(1.5f, 3f);
    public Vector2 retreatTimeRange = new Vector2(1f, 2f);

    private Transform player;
    private Rigidbody rb;

    private float stateTimer;
    private int orbitDir;

    enum EnemyState { Chase, Orbit, Retreat }
    private EnemyState currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        PickRandomState();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        stateTimer -= Time.fixedDeltaTime;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        float distance = toPlayer.magnitude;

        if (stateTimer <= 0f)
            PickRandomState();

        Vector3 moveDir = Vector3.zero;

        switch (currentState)
        {
            case EnemyState.Chase:
                if (distance > stoppingDistance)
                    moveDir = toPlayer.normalized * moveSpeed;
                break;

            case EnemyState.Orbit:
                Vector3 tangent =
                    Vector3.Cross(Vector3.up, toPlayer.normalized).normalized * orbitDir;

                moveDir = tangent * orbitSpeed;

                float offset = distance - stoppingDistance;
                moveDir += toPlayer.normalized * Mathf.Clamp(offset, -0.5f, 0.5f);
                break;

            case EnemyState.Retreat:
                moveDir = -toPlayer.normalized * retreatSpeed;
                break;
        }

        rb.linearVelocity = new Vector3(moveDir.x, 0f, moveDir.z);

        if (toPlayer != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(toPlayer);
            rb.MoveRotation(
                Quaternion.Slerp(rb.rotation, lookRot, 8f * Time.fixedDeltaTime)
            );
        }
    }

    // ================= STATES =================

    void PickRandomState()
    {
        float r = Random.value;

        if (r < 0.4f) EnterChase();
        else if (r < 0.75f) EnterOrbit();
        else EnterRetreat();
    }

    void EnterChase()
    {
        currentState = EnemyState.Chase;
        stateTimer = Random.Range(chaseTimeRange.x, chaseTimeRange.y);
    }

    void EnterOrbit()
    {
        currentState = EnemyState.Orbit;
        stateTimer = Random.Range(orbitTimeRange.x, orbitTimeRange.y);
        orbitDir = Random.value > 0.5f ? 1 : -1;
    }

    void EnterRetreat()
    {
        currentState = EnemyState.Retreat;
        stateTimer = Random.Range(retreatTimeRange.x, retreatTimeRange.y);
    }
}
