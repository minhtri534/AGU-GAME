using System.Collections;
using UnityEngine;

public class WeaponReloadingState : WeaponState
{
    public WeaponReloadingState(WeaponController controller) : base(controller) { }

    public override void Enter()
    {
        controller.StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(controller.weaponData.reloadTime);
        controller.currentAmmo = controller.weaponData.magazineSize;
        controller.SetState(new WeaponIdleState(controller));
    }
}
