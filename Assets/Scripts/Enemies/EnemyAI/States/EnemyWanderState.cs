using System.Collections;
using UnityEngine;

public class EnemyWanderState : EnemyState
{
    private Vector3 currentRandomPos;
    public override void Enter()
    {
        enemy.GetComponent<Animator>().SetTrigger("Move");
        currentRandomPos = RandomLocation();
        enemy.StartCoroutine(Countdown());
    }

    public override void Update()
    {
        Vector3 distance = currentRandomPos - enemy.transform.position;
        distance.y = 0f;
        if (distance.magnitude <= 0.2)
        {
            enemy.GetComponent<Animator>().SetTrigger("Idle");
            distance = Vector3.zero;
        }
        else
        {
            enemy.GetComponent<Animator>().SetTrigger("Move");
        }

        Vector3 move = distance.normalized * enemy.stats.Speed;
        enemy.rb.linearVelocity = Vector3.Lerp(enemy.rb.linearVelocity, new Vector3(move.x, 0, move.z), Time.deltaTime * 7);
    }

    public override void Exit() { }

    private Vector3 RandomLocation()
    {
        return enemy.transform.position + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(4);
        currentRandomPos = RandomLocation();
    }
}
