using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip runningSound;
    public AudioClip jumpingSound;
    public AudioClip slidingSound;
    public AudioClip crouchingSound;
    public AudioClip potionSound;
    public AudioClip climbingSound;
    public AudioClip DashingSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayJumpSound()
    {
       
        if (jumpingSound != null)
        {
            audioSource.PlayOneShot(jumpingSound);
        }
    }
    public void PlayDashSound()
    {

        if (DashingSound != null)
        {
            audioSource.PlayOneShot(DashingSound);
        }
    }
    

    public void PlaySoundInLoop(AudioClip playerSound)
    {
        if (playerSound != null && !audioSource.isPlaying)
        {
            audioSource.clip = playerSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void PlayRunningSound()
    {
        if (runningSound != null)
        {
            audioSource.clip = runningSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void PlayShootSound(AudioClip shootSound)
    {
        if(shootSound != null)
        {
            audioSource.PlayOneShot(shootSound, 0.2f);
        }
    }
    public void StopRunningSound()
    {
        if (audioSource.clip == runningSound && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioSource.loop = false;
        }
    }
    

}
