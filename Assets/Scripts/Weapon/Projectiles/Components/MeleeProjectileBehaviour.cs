using System;
using UnityEngine;

public class MeleeProjectileBehaviour : BaseProjectileComponent
{
    private Vector3 originalPos;
    private Quaternion originalAngle;
    private float timeElapsed = 0;
    private Vector3 tempVelocity;
    public override bool OnShot(Projectile p)
    {
        originalPos = p.ProjectileOwner.transform.position;
        originalAngle = p.ProjectileOwner.AimTarget.Aim(p.ProjectileOwner.firePoint.position);
        tempVelocity = p.transform.forward * p.Speed;
        return false;
    }
    
    public override bool OnTraveling(Projectile p)
    {
        
        var distance = originalPos - p.ProjectileOwner.transform.position;
        p.transform.position -= distance;

        var tempCurrentRotation = p.ProjectileOwner.AimTarget.Aim(p.ProjectileOwner.firePoint.position);
        // For some reason the difference of the y axis of the euler angles 
        // corresponds to the difference of the signed angle between the two quaternation with respect to the y axis
        float angleDifference = tempCurrentRotation.eulerAngles.y - originalAngle.eulerAngles.y;

        p.transform.RotateAround(p.ProjectileOwner.transform.position, Vector3.up, angleDifference);
        p.rb.linearVelocity = Quaternion.AngleAxis(angleDifference, Vector3.up) * p.rb.linearVelocity;
        tempVelocity = Quaternion.AngleAxis(angleDifference, Vector3.up) * tempVelocity;
        
        // updates stored gun position and rotation
        originalPos = p.ProjectileOwner.transform.position;
        originalAngle = tempCurrentRotation;

        timeElapsed += Time.deltaTime;
        p.rb.linearVelocity = tempVelocity * (p.GunStats.GetProjectileLifeTime() - timeElapsed) / p.GunStats.GetProjectileLifeTime();
        return false;
    }
}