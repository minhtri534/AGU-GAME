using UnityEngine;
using System.Collections;
using Photon.Pun;

public class HealerSkill : MonoBehaviourPun, IPlayerSkill
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
                if (photonView.IsMine) stats.UseMP(20f);
                StartCoroutine(HealCoroutine());
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

            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        isHealing = false;
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(5f);
        isCooldown = false;
    }
}