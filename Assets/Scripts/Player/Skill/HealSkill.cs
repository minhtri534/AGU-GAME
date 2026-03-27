using UnityEngine;
using System.Collections;

public class HealerSkill : MonoBehaviour, IPlayerSkill
{
    private RuntimeCharacterStats stats;
    private bool isHealing = false;
    private bool isCooldown = false;

    public void Activate()
    {
        if (stats == null)
        {
            var pc = GetComponent<PlayerController>();
            if (pc != null) stats = pc.GetStats();
        }

        if (!isHealing && !isCooldown && stats != null)
        {
            if (stats.CurrentMP >= 20f)
            {
                stats.UseMP(20f);
                StartCoroutine(HealCoroutine());
            }
            else
            {
                Debug.Log("[Healer] Not enough MP!");
            }
        }
    }

    IEnumerator HealCoroutine()
    {
        isHealing = true;

        float duration = 10f;
        float timer = 0f;
        float healAmountPerSecond = 20f;

        while (timer < duration)
        {
            if (stats == null) break;

            float newHP = stats.CurrentHP + healAmountPerSecond;
            stats.SetCurrentHP(newHP);

            Debug.Log($"[Healer] Healing... Current HP: {stats.CurrentHP}");

            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        isHealing = false;
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        Debug.Log("[Healer] Skill is on Cooldown...");

        yield return new WaitForSeconds(5f);

        isCooldown = false;
        Debug.Log("[Healer] Skill Ready!");
    }
}