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
            else
            {
                if (photonView.IsMine) Debug.Log($"[Healer] FAILED: {photonView.Owner.NickName} not enough MP!");
            }
        }
    }

    IEnumerator HealCoroutine()
    {
        isHealing = true;
        float duration = 10f;
        float timer = 0f;
        float healAmountPerSecond = 20f;

        Debug.Log($"[Healer] SKILL START: Player {photonView.Owner.NickName} started healing at {Time.time}s. Target HP: {stats.CurrentHP}");

        while (timer < duration)
        {
            if (stats == null) break;

            float newHP = stats.CurrentHP + healAmountPerSecond;
            stats.SetCurrentHP(newHP);

            // Log mỗi nhịp hồi máu nếu bạn muốn theo dõi sát (có thể bỏ dòng này nếu quá nhiều log)
            // Debug.Log($"[Healer] TICK: {photonView.Owner.NickName} HP is now {stats.CurrentHP}");

            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        Debug.Log($"[Healer] SKILL END: Healing finished for {photonView.Owner.NickName} at {Time.time}s. Final HP: {stats.CurrentHP}");

        isHealing = false;
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        float cooldownTime = 5f;
        Debug.Log($"[Healer] COOLDOWN: Started for {photonView.Owner.NickName} at {Time.time}s (Duration: {cooldownTime}s)");

        yield return new WaitForSeconds(cooldownTime);

        isCooldown = false;
        Debug.Log($"[Healer] READY: Player {photonView.Owner.NickName} skill is ready at {Time.time}s");
    }
}