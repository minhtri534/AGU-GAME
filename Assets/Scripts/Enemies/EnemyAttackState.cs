using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private Gun gun;
    private Coroutine attackCoroutine;
    public void Enter(EnemyController enemy)
    {
        enemy.stateTimer = Random.Range(
            enemy.chaseTimeRange.x,
            enemy.chaseTimeRange.y
        );
        gun = enemy.GetComponent<Gun>();
        var aim = (GunAimTargetEnemy)gun.AimTarget;
        aim.targetPos = enemy.player;
        attackCoroutine = enemy.StartCoroutine(Attack(enemy));
    }

    public void Update(EnemyController enemy)
    {
        Vector3 distance = enemy.player.position - enemy.transform.position;
        distance.y = 0f;

        Vector3 move = ((distance.magnitude - enemy.statsData.range) * distance).normalized * enemy.stats.Speed;
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, new Vector3(move.x, 0, move.z), Time.deltaTime * 7);
    }

    private IEnumerator Attack(EnemyController enemy)
    {
        ((GunInputEnemy)gun.GunInput).Click();
        yield return new WaitForSeconds(gun.stats.GetReloadTime());
        attackCoroutine = enemy.StartCoroutine(Attack(enemy));
    }

    public void Exit(EnemyController enemy)
    {
        enemy.StopCoroutine(attackCoroutine);
    }
}
