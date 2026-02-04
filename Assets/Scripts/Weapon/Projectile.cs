using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;
    public float lifetime = 5f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (rb != null)
        {
            rb.velocity = transform.forward * speed;
        }
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Basic hook: extend to apply damage to targets with your own health system
        Destroy(gameObject);
    }
}
