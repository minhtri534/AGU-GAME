using System;
using Unity.VisualScripting;
using UnityEngine;

public class ExampleProjectileBehaviour : BaseProjectileComponent
{
    // Example class that implements some random projectile behaviours
    private Vector3 directionOffset;
    private float rotationSpeed = 400;
    private float speed;
    // Fires a second bullet
    public override bool OnShot(Projectile p)
    {
        var new_p = Instantiate(p);

        // get projectile direction and speed
        speed = p.speed * 0.5f;
        directionOffset = p.transform.right * speed;
        
        return false;
    }
    
    // Makes bullet travel in a circular path
    public override bool OnTraveling(Projectile p)
    {
        directionOffset = Quaternion.Euler(0, Time.deltaTime * rotationSpeed, 0) * directionOffset;
        p.rb.linearVelocity = directionOffset + p.speed * p.transform.forward;
        return false;
    }

    // Creates 6 split bullets on break
    public override bool OnBreak(Projectile p)
    {
        for (int i = 0;i < 6; i++)
        {
            var new_p = Instantiate(p);
            // move the new bullet slightly so that it doesnt collide again immediately
            new_p.transform.Translate(-p.transform.forward);
            new_p.transform.Rotate(Vector3.up, 60 * i);
             
        }
        return false;
    }
}