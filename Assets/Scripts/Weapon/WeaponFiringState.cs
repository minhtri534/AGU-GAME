using System.Collections;
using UnityEngine;

public class WeaponFiringState : WeaponState
{
    private Coroutine firingRoutine;

    public WeaponFiringState(WeaponController controller) : base(controller) { }

    public override void Enter()
    {
        if (controller.CurrentIsAutomatic)
        {
            firingRoutine = controller.StartCoroutine(AutomaticFire());
        }
        else
        {
            // semi-auto: one shot and return to idle or reload
            controller.TryFireOnce();
            EvaluatePostFire();
        }
    }

    private IEnumerator AutomaticFire()
    {
        while (true)
        {
            controller.TryFireOnce();
            if (controller.currentAmmo <= 0)
            {
                break;
            }
            var wait = controller.GetFireInterval();
            yield return new WaitForSeconds(wait);
            if (!Input.GetMouseButton(0)) break;
        }
        EvaluatePostFire();
    }

    private void EvaluatePostFire()
    {
        if (controller.currentAmmo <= 0)
        {
            controller.SetState(new WeaponReloadingState(controller));
        }
        else
        {
            controller.SetState(new WeaponIdleState(controller));
        }
    }

    public override void Exit()
    {
        if (firingRoutine != null)
        {
            controller.StopCoroutine(firingRoutine);
            firingRoutine = null;
        }
    }
}
