using UnityEngine;

public class DamageOverTimeEffect : MonoBehaviour
{
    private CharacterController target;
    private float damagePerTick;
    private float tickInterval;
    private float timeRemaining;
    private float tickTimer;

    public void Initialize(CharacterController newTarget, float totalDamage, float durationSeconds, float tickIntervalSeconds)
    {
        target = newTarget;
        tickInterval = Mathf.Max(0.05f, tickIntervalSeconds);
        timeRemaining = Mathf.Max(0.05f, durationSeconds);

        var tickCount = Mathf.Max(1, Mathf.RoundToInt(timeRemaining / tickInterval));
        damagePerTick = totalDamage / tickCount;

        tickTimer = 0f;
    }

    private void Update()
    {
        if (target == null || target.stats == null)
        {
            Destroy(this);
            return;
        }

        timeRemaining -= Time.deltaTime;
        tickTimer -= Time.deltaTime;

        if (tickTimer <= 0f)
        {
            tickTimer = tickInterval;
            target.stats.TakeDamage(damagePerTick);
        }

        if (timeRemaining <= 0f)
        {
            Destroy(this);
        }
    }
}
