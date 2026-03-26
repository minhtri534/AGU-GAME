using UnityEngine;

public class GunAimTargetEnemy : IGunAimTarget
{
    public Transform targetPos;
    /// <summary>
    /// Returns the rotation to the target position
    /// </summary>
    /// <returns></returns>
    public virtual Quaternion Aim(Vector3 gunPos)
    {
        Vector3 direction = targetPos.position - gunPos;
        direction.y = 0; // Giữ đạn bay ngang, không cắm đầu xuống đất

        if (direction == Vector3.zero) return Quaternion.identity;

        return Quaternion.LookRotation(direction);
    }
}

enum EnemyTargettingType
{
    Nearest,
    Furthest,
    Weakest,
    Strongest,
}