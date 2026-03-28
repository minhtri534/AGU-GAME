using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Projectile class for all projectiles
/// </summary>

[RequireComponent(typeof(Rigidbody))] // Tự động thêm Rigidbody nếu chưa có
public class Projectile : MonoBehaviour, IPunInstantiateMagicCallback
{
    public float Speed;
    public float LifeTime;
    public float Size;
    public float Damage;
    public GunStats GunStats;
    public Rigidbody rb;
    public Gun ProjectileOwner;
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
        
        OnShot();
    }

    // Called on remote instances when created via PhotonNetwork.Instantiate
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var pv = GetComponent<PhotonView>();
        if (pv != null && pv.InstantiationData != null)
        {
            object[] d = pv.InstantiationData;
            try
            {
                if (d.Length >= 6)
                {
                    int ownerViewId = Convert.ToInt32(d[0]);
                    Damage = Convert.ToSingle(d[1]);
                    Speed = Convert.ToSingle(d[2]);
                    LifeTime = Convert.ToSingle(d[3]);
                    Size = Convert.ToSingle(d[4]);
                    bool isEnemy = Convert.ToBoolean(d[5]);

                    // Try to find owner Gun by PhotonView id
                    try
                    {
                        var ownerPV = PhotonView.Find(ownerViewId);
                        if (ownerPV != null)
                        {
                            ProjectileOwner = ownerPV.GetComponent<Gun>();
                        }
                    }
                    catch { }

                    var col = GetComponent<Collider>();
                    if (col != null)
                    {
                        if (isEnemy) col.excludeLayers += LayerMask.GetMask("Enemy");
                        else col.excludeLayers += LayerMask.GetMask("Player");
                    }
                    var mr = GetComponent<MeshRenderer>();
                    if (mr != null && isEnemy) mr.material = Resources.Load<Material>("Materials/BulletEnemy");
                }
            }
            catch { }
        }
    }
    void Update()
    {
        OnTraveling();
    }

    /*void OnCollisionEnter(Collision collision)
    {
        // 1. Kiểm tra xem đạn có trúng Enemy không
        // Thử tìm component EnemyController trên vật thể bị bắn trúng
        CharacterController enemy = collision.gameObject.GetComponent<CharacterController>();
        
        // Nếu không thấy, thử tìm ở object cha (đôi khi collider nằm ở con, script nằm ở cha)
        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponentInParent<CharacterController>();
        }

        OnHit(enemy);
    }*/

    void OnTriggerEnter(Collider other)
    {
        // 1. Kiểm tra xem đạn có trúng Enemy không
        // Thử tìm component EnemyController trên vật thể bị bắn trúng
        CharacterController enemy = other.gameObject.GetComponent<CharacterController>();
        
        // Nếu không thấy, thử tìm ở object cha (đôi khi collider nằm ở con, script nằm ở cha)
        if (enemy == null)
        {
            enemy = other.gameObject.GetComponentInParent<CharacterController>();
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
    private void OnHit(CharacterController target) // update this to include the player as well
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
