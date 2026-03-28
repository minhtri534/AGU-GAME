using System.Collections;
using UnityEngine;

public class EnemySmartAttackState : EnemyState
{
    private Gun gun;
    private Coroutine attackCoroutine;
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Move");
        gun = enemy.GetComponent<Gun>();
        var aim = (GunAimTargetEnemy)gun.AimTarget;
        aim.targetPos = enemy.player;
        attackCoroutine = enemy.StartCoroutine(Attack());
    }

    public override void Update()
    {
        Vector3 distance = enemy.player.position - enemy.transform.position;
        distance.y = 0f;

        Vector3 move = ((distance.magnitude - enemy.statsData.range) * distance).normalized * enemy.stats.Speed;
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, new Vector3(move.x, 0, move.z), Time.deltaTime * 7);
    }

    private IEnumerator Attack()
    {
        ((GunInputEnemy)gun.GunInput).Click();
        yield return new WaitForSeconds(gun.stats.GetReloadTime());
        attackCoroutine = enemy.StartCoroutine(Attack());
    }

    public override void Exit()
    {
        enemy.StopCoroutine(attackCoroutine);
    }
}
