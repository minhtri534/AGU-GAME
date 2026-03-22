using UnityEngine;
using UnityEngine.InputSystem;

public interface IGunAimTarget
{
    public Quaternion Aim(Vector3 gunPos);
}