using UnityEngine;
using UnityEngine.InputSystem;

public class GunAimTargetPlayer : IGunAimTarget
{
    /// <summary>
    /// Returns the rotation to the mouse position
    /// </summary>
    /// <returns></returns>
    public Quaternion Aim(Vector3 gunPos)
    {
        if (Camera.main == null) return Quaternion.identity; // Kiểm tra camera

        // SỬA LỖI: Dùng Mouse.current.position thay vì Input.mousePosition
        Vector2 mouseScreenPos = Vector2.zero;
        if (Mouse.current != null)
        {
            mouseScreenPos = Mouse.current.position.ReadValue();
        }

        // 1. Tính toán điểm người chơi đang click chuột
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        // Tạo mặt phẳng ảo ngang tầm với súng (để chuột luôn nằm trên mặt phẳng này)
        Plane plane = new(Vector3.up, gunPos);

        Vector3 hitPoint = Vector3.zero;
        if (plane.Raycast(ray, out float enter))
        {
            hitPoint = ray.GetPoint(enter);
        }
        else
        {
            // Fallback nếu click ra ngoài trời
            hitPoint = ray.GetPoint(50f);
        }

        // 2. Tính hướng từ súng tới điểm click
        Vector3 direction = hitPoint - gunPos;
        direction.y = 0; // Giữ đạn bay ngang, không cắm đầu xuống đất

        if (direction == Vector3.zero) return Quaternion.identity;

        return Quaternion.LookRotation(direction);
    }
}