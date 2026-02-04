using UnityEngine;

public abstract class WeaponState
{
    protected WeaponController controller;
    public WeaponState(WeaponController controller) { this.controller = controller; }
    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void HandleInput() { }
    public virtual void StateUpdate() { }
}
