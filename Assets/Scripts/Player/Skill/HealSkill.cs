using UnityEngine;
using System.Collections;

public class HealerSkill : MonoBehaviour, IPlayerSkill
{
    private RuntimeCharacterStats stats;
    private bool isHealing = false;

    void Awake()
    {
        stats = GetComponent<PlayerController>().GetStats();
    }

    public void Activate()
    {
        if (!isHealing)
            StartCoroutine(HealCoroutine());
    }

    IEnumerator HealCoroutine()
    {
        isHealing = true;

        float duration = 10f;
        float timer = 0f;

        while (timer < duration)
        {
            if (stats.CurrentMP < 20f)
                break;

            stats.UseMP(20f);

            float healAmount = 20f;

            float newHP = stats.CurrentHP + healAmount;

            newHP = Mathf.Min(newHP, stats.MaxHP);

            stats.SetCurrentHP(newHP);

            Debug.Log($"[Healer] HP: {stats.CurrentHP}");

            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        isHealing = false;
    }
}
