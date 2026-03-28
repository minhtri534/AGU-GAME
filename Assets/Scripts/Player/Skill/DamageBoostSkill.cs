using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BerserkerSkill : MonoBehaviourPun, IPlayerSkill
{
    private Gun gun;
    private GunStats gunStats;
    private bool isActive = false;
    private bool isCooldown = false;

    void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        if (gun != null) gunStats = gun.GetStats();
    }

    public void Activate()
    {
        if (!isActive && !isCooldown && gunStats != null)
        {
            StartCoroutine(BuffCoroutine());
        }
    }

    IEnumerator BuffCoroutine()
    {
        isActive = true;
        float bonusDamage = 30f;
        float originalDamage = gunStats.GetDamage();

        Debug.Log($"[Berserker] SKILL START: Player {photonView.Owner.NickName} activated buff at {Time.time}s. Damage: {originalDamage} -> {originalDamage + bonusDamage}");

        gunStats.SetDamage(originalDamage + bonusDamage);

        yield return new WaitForSeconds(5f);

        gunStats.SetDamage(originalDamage);
        Debug.Log($"[Berserker] SKILL END: Buff expired for {photonView.Owner.NickName} at {Time.time}s. Damage reset to {originalDamage}");

        isActive = false;
        StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        float cooldownTime = 5f;
        Debug.Log($"[Berserker] COOLDOWN: Started for {photonView.Owner.NickName} at {Time.time}s (Duration: {cooldownTime}s)");

        yield return new WaitForSeconds(cooldownTime);

        isCooldown = false;
        Debug.Log($"[Berserker] READY: Player {photonView.Owner.NickName} skill is ready at {Time.time}s");
    }
}