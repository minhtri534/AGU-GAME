using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        enemy.stateTimer = Random.Range(
            enemy.chaseTimeRange.x,
            enemy.chaseTimeRange.y
        );
    }

    public void Update(EnemyController enemy)
    {
        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f;

        float distance = toPlayer.magnitude;

        if (distance > enemy.stoppingDistance)
        {
            Vector3 move = toPlayer.normalized * enemy.moveSpeed;
            enemy.rb.linearVelocity = new Vector3(move.x, 0, move.z);
        }
    }

    public void Exit(EnemyController enemy) { }
}
