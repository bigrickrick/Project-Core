using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip Running;
    public AudioClip Jumping;
    public AudioClip Sliding;
    public AudioClip CroutchWalking;
    public AudioClip TakePotionsound;
    public AudioClip ClimbingSound;

    public AudioSource audioSource;
    public void PLayPlayerSoundOnce(AudioClip Playersound)
    {
        if (Playersound != null)
        {

            audioSource.clip = Playersound;
            audioSource.Play();
            Destroy(audioSource, Playersound.length + 0.1f);
        }
    }
    
    
    public void PlaySoundInLoop(AudioClip playerSound)
    {
        if (playerSound != null)
        {
            audioSource.clip = playerSound;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
    public void StopLoopedSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false; 
        }
    }
    private void Update()
    {
        if(Player.Instance.state == Player.MovementState.walking)
        {
            
            PlaySoundInLoop(Running);
        }
        else if(Player.Instance.state == Player.MovementState.Croutch && Player.Instance.GetComponent<Sliding>().sliding)
        {
            
            PlaySoundInLoop(Sliding);
        }
        else if (Player.Instance.state == Player.MovementState.Croutch)
        {

            
            PlaySoundInLoop(CroutchWalking);
        }
        else if (Player.Instance.state == Player.MovementState.wallrunning)
        {
            
            PlaySoundInLoop(Running);
        }
        else if (Player.Instance.state == Player.MovementState.climbing)
        {
            
            PlaySoundInLoop(ClimbingSound);
        }
        if(Player.Instance.isJumping && Player.Instance.isGrounded)
        {
            
            PLayPlayerSoundOnce(Jumping);
        }
        StopLoopedSound();
    }
}
