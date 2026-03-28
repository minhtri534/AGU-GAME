using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemySmartChaseState : EnemyState
{
    public float DistanceToEnemyRange = 3f;
    public EnemySmartChaseState(float distanceToEnemyRange)
    {
        DistanceToEnemyRange = distanceToEnemyRange;
    }
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Move");
    }

    public override void Update()
    {
        Vector3 distanceToPlayer = enemy.player.position - enemy.transform.position;
        distanceToPlayer.y = 0f;

        Vector3 sumDistanceToEnemies = new();

        Vector3 finalDistance = distanceToPlayer;

        var colliders = Physics.OverlapSphere(enemy.transform.position, DistanceToEnemyRange);
        foreach (var col in colliders)
        {
            var otherEnemy = col.gameObject.GetComponent<EnemyController>();
            if (otherEnemy == null || otherEnemy == enemy)
            {
                continue;
            }
            sumDistanceToEnemies += enemy.transform.position - otherEnemy.transform.position;
        }

        if (distanceToPlayer.magnitude >= DistanceToEnemyRange * 0.7)
        {
            finalDistance += sumDistanceToEnemies;
        }

        Vector3 move = finalDistance.normalized * enemy.stats.Speed;
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, new Vector3(move.x, 0, move.z), Time.deltaTime * 7);
    }

    public override void Exit()
    {
    }
}
