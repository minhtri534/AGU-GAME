using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile class for all projectiles
/// </summary>

[RequireComponent(typeof(Rigidbody))] // Tự động thêm Rigidbody nếu chưa có
public class Projectile : MonoBehaviour
{
    public float Speed;
    public float LifeTime;
    public float Size;
    public float Damage;
    public Rigidbody rb;
    private bool isOnBreakQueued = false;

    private BaseProjectileComponent defaultBehaviour;
    private List<BaseProjectileComponent> projectileComponents = new();
    public void addProjectileComponent(BaseProjectileComponent c)
    {
        projectileComponents.Add(c);
    }

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
    void Update()
    {
        OnTraveling();
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
        gameObject.transform.localScale *= Size;
        StartCoroutine(DestroyAfterLifeTime());
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

    /// <summary>
    /// Called when the projectile breaks after collision
    /// </summary>
    /// <remarks>
    /// Makes sure that OnHit() is only called once
    /// </remarks>
    public void QueueOnBreak()
    {
        isOnBreakQueued = true;
    }

    private IEnumerator DestroyAfterLifeTime()
    {
        yield return new WaitForSeconds(LifeTime);
        QueueOnBreak();
    }
}
