using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Move");
    }

    public override void Update()
    {
        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f;

        Vector3 move = toPlayer.normalized * enemy.stats.Speed;
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, new Vector3(move.x, 0, move.z), Time.deltaTime * 7);

    }


    public override void Exit() { }
}
