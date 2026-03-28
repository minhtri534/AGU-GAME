using UnityEngine;

public class EnemyOrbitState : EnemyState
{
    public float StoppingDistance = 1.5f;
    public EnemyOrbitState(float stoppingDistance)
    {
        StoppingDistance = stoppingDistance;
    }
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Move");
    }

    public override void Update()
    {
        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f;

        Vector3 tangent =
            Vector3.Cross(Vector3.up, toPlayer.normalized);

        Vector3 move = tangent.normalized * enemy.statsData.orbitSpeed;

        float offset = toPlayer.magnitude - StoppingDistance;
        move += toPlayer.normalized * Mathf.Clamp(offset, -0.5f, 0.5f);

        enemy.rb.linearVelocity = new Vector3(move.x, 0, move.z);
    }

    public override void Exit() { }
}
