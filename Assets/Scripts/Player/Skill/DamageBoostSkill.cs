using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BerserkerSkill : MonoBehaviourPun, IPlayerSkill
{
    private Gun gun;
    private GunStats gunStats;
    private bool isActive = false;

    void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        if (gun != null) gunStats = gun.GetStats();
    }

    public void Activate()
    {
        if (!isActive && gunStats != null)
            StartCoroutine(BuffCoroutine());
    }

    IEnumerator BuffCoroutine()
    {
        isActive = true;
        float bonusDamage = 30f;
        float originalDamage = gunStats.GetDamage();

        gunStats.SetDamage(originalDamage + bonusDamage);

        yield return new WaitForSeconds(5f);

        gunStats.SetDamage(originalDamage);
        isActive = false;
    }
}