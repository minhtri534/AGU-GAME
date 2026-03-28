using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Idle");
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }
}