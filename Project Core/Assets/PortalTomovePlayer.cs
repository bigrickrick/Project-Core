using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTomovePlayer : MonoBehaviour
{
    public GameObject location; // Use lowercase for variable names by convention

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the portal trigger is the player
        if (other.CompareTag("Player"))
        {
            // Move the player to the location of the portal
            MovePlayer(location.transform.position);
        }
    }

    private void MovePlayer(Vector3 newPosition)
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Set the player's position to the new position
            player.transform.position = newPosition;
        }
        
    }
}
