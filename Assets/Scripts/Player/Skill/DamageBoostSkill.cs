using UnityEngine;
using System.Collections;

public class BerserkerSkill : MonoBehaviour, IPlayerSkill
{
    private Gun gun;
    private GunStats gunStats;
    private bool isActive = false;

    void Awake()
    {
        gun = GetComponentInChildren<Gun>();  
        gunStats = gun.GetStats();           
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

        float originalDamage = gunStats.GetDamage();

        Debug.Log($"[Berserker] Gun Damage BEFORE: {originalDamage}");

        gunStats.SetDamage(originalDamage + bonusDamage);

        Debug.Log($"[Berserker] Gun Damage AFTER BUFF: {gunStats.GetDamage()}");

        yield return new WaitForSeconds(5f);

        gunStats.SetDamage(originalDamage);

        Debug.Log($"[Berserker] Gun Damage AFTER END: {gunStats.GetDamage()}");

        isActive = false;
    }
}