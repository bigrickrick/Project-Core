using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isOrange;
    public float distance = 0f;
    public float yOffset = 1.0f; // Adjust this value to set how high above the floor the player should spawn
    private Portal opposedPortal;
    private bool HasTeleported;
    public bool HasbeenMove;

    

    public Portal()
    {
        opposedPortal = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (HasbeenMove)
        {
            if (!HasTeleported)
            {
                //SetDistination();
                if (Vector3.Distance(transform.position, other.transform.position) > distance)
                {
                    HasTeleported = true;
                    //Debug.Log("desitnation " + destination.GetComponent<Portal>() == null);

                    
                    opposedPortal.HasTeleported = true;
                    Vector3 destinationPosition = opposedPortal.transform.position + Vector3.up;
                    other.transform.position = destinationPosition;
                }
            }
        }
        
        
    }
    public Portal getOpposedPortal()
    {
        return opposedPortal;
    }
    public void setOpposedPortal(Portal p)
    {
        opposedPortal = p;
    }
    private void SetDistination()
    {
        if (isOrange == false)
        {
            if (GameObject.FindGameObjectWithTag("OrangePortal") != null)
            {
                //
                //GameObject.FindGameObjectsWithTag("OrangePortal");

            }

        }
        else
        {
            if (GameObject.FindGameObjectWithTag("BluePortal") != null)
            {
                //this.opposedPortal = GameObject.FindGameObjectWithTag("BluePortal").GetComponent<Transform>();
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        HasTeleported = false;
    }

}
