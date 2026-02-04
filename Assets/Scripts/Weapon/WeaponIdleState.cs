using UnityEngine;

public class WeaponIdleState : WeaponState
{
    public WeaponIdleState(WeaponController controller) : base(controller) { }

    public override void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            controller.TryStartFiring();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            controller.StartReload();
        }
    }
}
