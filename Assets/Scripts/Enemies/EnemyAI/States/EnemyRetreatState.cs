using UnityEngine;

public class EnemyRetreatState : EnemyState
{
    public float RetreatSpeed = 4f;
    public EnemyRetreatState(float retreatSpeed)
    {
        RetreatSpeed = retreatSpeed;
    }
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Move");
    }

    public override void Update()
    {
        Vector3 toPlayer = enemy.player.position - enemy.transform.position;
        toPlayer.y = 0f;

        Vector3 move = -toPlayer.normalized * RetreatSpeed;
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, new Vector3(move.x, 0, move.z), Time.deltaTime * 7);
    }

    public override void Exit() { }
}
