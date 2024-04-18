using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTomovePlayer : MonoBehaviour
{
    public GameObject location; // Use lowercase for variable names by convention
    public AudioClip BossMusic;
    public MusicPlayer musicPlayer;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that entered the portal trigger is the player
        if (other.GetComponent<Player>())
        {
            // Move the player to the location of the portal
            MovePlayer(other.GetComponent<Player>());
        }
    }

    private void MovePlayer(Player player)
    {
        
       

        if (player != null)
        {

            player.transform.position = location.transform.position;
            musicPlayer.PlayMusic(BossMusic);
        }
        
    }
}
