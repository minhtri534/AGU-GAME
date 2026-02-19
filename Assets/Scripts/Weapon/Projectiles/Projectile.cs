using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] // Tự động thêm Rigidbody nếu chưa có
public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    public float damage;
    public Rigidbody rb;
    private bool isOnBreakQueued = false;

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
    private BaseProjectileComponent defaultBehaviour;
    private BaseProjectileComponent[] projectileComponents = {};

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultBehaviour = gameObject.AddComponent<DefaultProjectileBehaviour>();
        
        // Cấu hình vật lý cho đạn
        rb.useGravity = false; 
        
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
        OnShot();
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

        OnHit(enemy);
    }


    void LateUpdate()
    {
        if (isOnBreakQueued)
        {
            OnBreak();
        }
    }

    // Called whenever the projectile has just been fired
    private void OnShot()
    {
        var overrideDefault = false;
        foreach (var component in projectileComponents)
        {
            if (component.OnShot(this))
            {
                overrideDefault = true;
            }
        }
        if (!overrideDefault)
        {
            defaultBehaviour.OnShot(this);
        }
    }
    
    // Called every frame as the projectile is travelling
    private void OnTraveling()
    {
        var overrideDefault = false;
        foreach (var component in projectileComponents)
        {
            if (component.OnTraveling(this))
            {
                overrideDefault = true;
            }
        }
        if (!overrideDefault)
        {
            defaultBehaviour.OnTraveling(this);
        }
    }

    // Called when the projectile collides
    private void OnHit(EnemyController target) // update this to include the player as well
    {
        var overrideDefault = false;
        foreach (var component in projectileComponents)
        {
            if (component.OnHit(this, target))
            {
                overrideDefault = true;
            }
        }
        if (!overrideDefault)
        {
            defaultBehaviour.OnHit(this, target);
        }
    }

    // Called when the projectile breaks after collision
    private void OnBreak()
    {
        var overrideDefault = false;
        foreach (var component in projectileComponents)
        {
            if (component.OnBreak(this))
            {
                overrideDefault = true;
            }
        }
        if (!overrideDefault)
        {
            defaultBehaviour.OnBreak(this);
        }
        Destroy(gameObject);
    }

    // Make sure that OnHit() is only called once
    public void QueueOnBreak()
    {
        isOnBreakQueued = true;
    }
}
