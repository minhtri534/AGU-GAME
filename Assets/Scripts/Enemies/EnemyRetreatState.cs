using UnityEngine;

public class EnemyRetreatState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        enemy.stateTimer = Random.Range(
            enemy.retreatTimeRange.x,
            enemy.retreatTimeRange.y
        );
    }

    public void Update(EnemyController enemy)
    {
        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f;

        Vector3 move = -toPlayer.normalized * enemy.statsData.retreatSpeed;
        enemy.rb.linearVelocity = new Vector3(move.x, 0, move.z);
    }

    public void Exit(EnemyController enemy) { }
}
