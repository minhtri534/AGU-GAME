using UnityEngine;
using System.Collections;

public class BerserkerSkill : MonoBehaviour, IPlayerSkill
{
    private RuntimeCharacterStats stats;
    private bool isActive = false;

    void Awake()
    {
        stats = GetComponent<PlayerController>().GetStats();
    }

    public void Activate()
    {
        if (!isActive)
            StartCoroutine(BuffCoroutine());
    }

    IEnumerator BuffCoroutine()
    {
        isActive = true;

        float bonusDamage = 30f;

        Debug.Log($"[Berserker] Damage BEFORE: {stats.Damage}");

        stats.SetDamage(stats.Damage + bonusDamage);

        Debug.Log($"[Berserker] Damage AFTER BUFF: {stats.Damage}");

        yield return new WaitForSeconds(5f);

        stats.SetDamage(stats.Damage - bonusDamage);

        Debug.Log($"[Berserker] Damage AFTER END: {stats.Damage}");

        isActive = false;
    }

}
