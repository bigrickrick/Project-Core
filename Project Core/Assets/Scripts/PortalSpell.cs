using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpell : Spell
{
    
    public LayerMask whatisGround;
    public LayerMask whatisWall;
    public Portal portalPrefab;
    private Portal orangePortal;
    private Portal bluePortal;
    private Portal currentOrgangePortal;
    private Portal currentBluePortal;
    public enum PortalState
    {
        Organge,
        blue
    }
    public PortalState state = PortalState.Organge;
    
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
            Vector3 portalForward = hit.normal;
            float x = portalForward.x;
            portalForward.x = portalForward.z;
            portalForward.z = x;
            portalForward.x *= 90;
            portalForward.z *= 90;
            


            Debug.Log(portalForward);
            

            if(state == PortalState.Organge)
            {
                if(orangePortal == null)
                {
                    orangePortal = CreatePortal(portalPrefab, hit, portalForward);
                    orangePortal.transform.localEulerAngles = portalForward;
                    spell.spellProjectile.GetComponent<Portal>().HasbeenMove = true;
                    currentOrgangePortal = orangePortal;
                    state = PortalState.blue;
                }
                else
                {
                    Portal bluePortal = orangePortal.getOpposedPortal();

                    Destroy(orangePortal.gameObject);
                    
                    orangePortal = null;


                    orangePortal = CreatePortal(portalPrefab, hit, portalForward);
                    orangePortal.setOpposedPortal(bluePortal);
                    bluePortal.setOpposedPortal(orangePortal);
                    orangePortal.transform.localEulerAngles = portalForward;
                    spell.spellProjectile.GetComponent<Portal>().HasbeenMove = true;
                    state = PortalState.blue;
                }
                
                
            }
            else
            {
                if(orangePortal.getOpposedPortal() == null)
                {
                    //CreatePortal(spell.spellProjectile.GetComponent<Portal>(), hit, portalForward);
                    bluePortal = CreatePortal(spell.spellProjectile.GetComponent<Portal>(), hit, portalForward);
                    bluePortal.transform.localEulerAngles = portalForward;
                    orangePortal.setOpposedPortal(bluePortal);
                    bluePortal.setOpposedPortal(orangePortal);

                    portalPrefab.HasbeenMove = true;
                    state = PortalState.Organge;
                }
                else
                {
                    Destroy(orangePortal.getOpposedPortal().gameObject);
                    orangePortal.setOpposedPortal(null);
                    

                    Portal bluePortal = CreatePortal(spell.spellProjectile.GetComponent<Portal>(), hit, portalForward);
                    bluePortal.transform.localEulerAngles = portalForward;
                    orangePortal.setOpposedPortal(bluePortal);
                    bluePortal.setOpposedPortal(orangePortal);

                    portalPrefab.HasbeenMove = true;
                    state = PortalState.Organge;
                }
                
            }
             

            




        }
    }

    private Portal CreatePortal(Portal portal, RaycastHit hit,Vector3 portalForward)
    {
        Portal p = Instantiate(portal, hit.point, Quaternion.identity);
        
       
        return p;
        
    }
    private void MovePortal(Portal portal, RaycastHit hit, Vector3 portalForward)
    {
        portal.transform.position = hit.point;
        portal.transform.localEulerAngles = portalForward;
    }
    

}
