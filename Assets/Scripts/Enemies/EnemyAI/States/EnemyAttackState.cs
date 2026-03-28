using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private Gun gun;
    private Coroutine attackCoroutine;
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Idle");
        gun = enemy.GetComponent<Gun>();
        var aim = (GunAimTargetEnemy)gun.AimTarget;
        aim.targetPos = enemy.player;
        attackCoroutine = enemy.StartCoroutine(Attack());
    }

    public override void Update()
    {
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
