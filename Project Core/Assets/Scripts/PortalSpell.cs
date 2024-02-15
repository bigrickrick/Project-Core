using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpell : Spell
{
    private bool OrangePortalhere;
    public LayerMask whatisGround;
    public LayerMask whatisWall;
    public Portal portalPrefab;
    
    public override void ShootSpell(Transform firepoint)
    {
        FirePortal(0, Player.Instance.transform.position, firepoint.transform.forward, 250f);
        
        
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        int layerMask = whatisGround.value | whatisWall.value;
        Physics.Raycast(pos, dir, out hit, distance, layerMask);

        if (Physics.Raycast(pos, dir, out hit, distance, layerMask))
        {
            //Vector3 portalForward = -hit.normal;

            
            //Vector3 portalRight = Vector3.Cross(Vector3.down, portalForward).normalized;

            
            //Vector3 portalUp = Vector3.Cross(portalForward, portalRight).normalized;


            Vector3 portalForward = hit.normal;
            float x = portalForward.x;
            portalForward.x = portalForward.z;
            portalForward.z = x;
            portalForward.x *= 90;
            portalForward.z *= 90;
            


            Debug.Log(portalForward);
            //Quaternion portalRotation = por

            //Quaternion.
            //Quaternion portalRotation = Quaternion.LookRotation(portalForward, Vector3.up);

            
            Portal newPortal = Instantiate(portalPrefab, hit.point, Quaternion.identity);

            newPortal.transform.localEulerAngles = portalForward;




        }
    }
    

}
