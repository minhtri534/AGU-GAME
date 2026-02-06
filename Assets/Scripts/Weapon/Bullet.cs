using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Tự động thêm Rigidbody nếu chưa có
public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public float damage = 10f; // Sát thương của đạn

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        // Cấu hình vật lý cho đạn
        rb.useGravity = false; 
        
        // Đẩy đạn bay theo hướng trước mặt (Z-axis) của nó
        // Lưu ý: Nếu Unity báo lỗi 'linearVelocity', hãy đổi thành 'velocity'
        rb.linearVelocity = transform.forward * speed; 

        // Tự động hủy sau một thời gian
        Destroy(gameObject, lifeTime);
        
        // Bỏ qua va chạm với Player (để không bị kẹt khi mới bắn ra)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider playerCollider = player.GetComponent<Collider>();
            Collider myCollider = GetComponent<Collider>();
            if (playerCollider != null && myCollider != null)
            {
                Physics.IgnoreCollision(playerCollider, myCollider);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 1. Kiểm tra xem đạn có trúng Enemy không
        // Thử tìm component EnemyController trên vật thể bị bắn trúng
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
        
        // Nếu không thấy, thử tìm ở object cha (đôi khi collider nằm ở con, script nằm ở cha)
        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        }

        // 2. Nếu đúng là Enemy thì trừ máu
        if (enemy != null && enemy.stats != null)
        {
            enemy.stats.TakeDamage(damage);
            Debug.Log("Hit Enemy! HP còn: " + enemy.stats.CurrentHP);

            // 3. Nếu máu về 0 thì tiêu diệt Enemy
            if (enemy.stats.IsDead)
            {
                Destroy(enemy.gameObject);
                Debug.Log("Enemy Dead!");
            }
        }

        // Cuối cùng, hủy viên đạn
        Destroy(gameObject);
    }
}
