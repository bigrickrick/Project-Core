using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalProjectile : Projectile
{
    public Portal portalPrefab;
    
    public LayerMask whatisGround;
    public LayerMask whatisWall;
    private Quaternion defaultRotation;
    public Transform portalPosition; // Changed from private to public

    public override void ApplyEffect()
    {
        if (portalPosition != null) // Check if portalPosition is assigned
        {
            GameObject portal = Instantiate(portalPrefab.gameObject, portalPosition.position, defaultRotation);
            // You might want to do something with 'portal' here
        }
        else
        {
            Debug.LogError("Portal position is not assigned!");
        }
    }

    public override void OnTriggerEnter(Collider other)
    {

        if ((whatisGround.value & 1 << other.gameObject.layer) != 0 ||
            (whatisWall.value & 1 << other.gameObject.layer) != 0) // Combining both checks
        {
            defaultRotation = other.transform.rotation;
            if (portalPosition == null) // Initialize portalPosition if it's null
            {
                portalPosition = other.transform; // or any other appropriate transform
            }
            portalPosition.position = other.ClosestPointOnBounds(transform.position);
            ApplyEffect();
        }

        Destroy(gameObject);
    }

}
