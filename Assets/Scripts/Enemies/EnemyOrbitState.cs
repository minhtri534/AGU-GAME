using UnityEngine;

public class EnemyOrbitState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        enemy.stateTimer = Random.Range(
            enemy.orbitTimeRange.x,
            enemy.orbitTimeRange.y
        );

        enemy.orbitDir = Random.value > 0.5f ? 1 : -1;
    }

    public void Update(EnemyController enemy)
    {
        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f;

        Vector3 tangent =
            Vector3.Cross(Vector3.up, toPlayer.normalized) * enemy.orbitDir;

        Vector3 move = tangent.normalized * enemy.orbitSpeed;

        float offset = toPlayer.magnitude - enemy.stoppingDistance;
        move += toPlayer.normalized * Mathf.Clamp(offset, -0.5f, 0.5f);

        enemy.rb.linearVelocity = new Vector3(move.x, 0, move.z);
    }

    public void Exit(EnemyController enemy) { }
}
