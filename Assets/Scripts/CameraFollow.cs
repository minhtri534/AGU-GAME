using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 15, -10); // Khoảng cách cố định

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        // Nếu chưa có Player thì không làm gì cả
        if (target == null) return;

        // Gán thẳng vị trí Camera = Vị trí Player + Khoảng cách
        transform.position = target.position + offset;

        // Nếu bạn muốn Camera luôn nhìn thẳng vào Player
        transform.LookAt(target);
    }

    // Hàm để Player gọi khi vừa Spawn
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}