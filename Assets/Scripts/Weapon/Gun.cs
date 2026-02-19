using UnityEngine;
using UnityEngine.InputSystem; // BẮT BUỘC: Thêm dòng này để dùng hệ thống Input mới

public class Gun : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab; // Kéo Prefab viên đạn vào đây
    public Transform firePoint;     // Kéo GameObject vị trí đầu nòng súng vào đây
    private GunInput gunInput;
    private PlayerController player;
    private

    void Awake()
    {
        player = GetComponent<PlayerController>();
        gunInput = gameObject.AddComponent<GunInput>();
    }

    void Update()
    {
        switch (gunInput.GetInput())
        {
            case GunInputState.None:

                break;
            case GunInputState.JustPressed:
                Shoot();
                break;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        if (Camera.main == null) return; // Kiểm tra camera

        // SỬA LỖI: Dùng Mouse.current.position thay vì Input.mousePosition
        Vector2 mouseScreenPos = Vector2.zero;
        if (Mouse.current != null)
        {
            mouseScreenPos = Mouse.current.position.ReadValue();
        }

        // 1. Tính toán điểm người chơi đang click chuột
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        
        // Tạo mặt phẳng ảo ngang tầm với súng (để chuột luôn nằm trên mặt phẳng này)
        Plane plane = new Plane(Vector3.up, firePoint.position);
        
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
        Vector3 direction = hitPoint - firePoint.position;
        direction.y = 0; // Giữ đạn bay ngang, không cắm đầu xuống đất

        if (direction == Vector3.zero) return;

        // 3. Xoay viên đạn theo hướng đó
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 4. Sinh ra viên đạn
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, rotation);

        Projectile bullet = bulletObj.GetComponent<Projectile>();

        if (bullet != null)
        {
            float playerDamage = player.GetStats().Damage;
            bullet.SetDamage(playerDamage);
            bullet.addProjectileComponent(gameObject.AddComponent<ExampleProjectileBehaviour>());
        }
    }
}
