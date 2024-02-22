using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip Running;
    public AudioClip Jumping;
    public AudioClip Sliding;
    public AudioClip TakePotionsound;

    public AudioSource audioSource;
    public void PLayPlayerSound(AudioClip Playersound)
    {
        if (Playersound != null)
        {

            audioSource.clip = Playersound;
            audioSource.Play();
            Destroy(audioSource, Playersound.length + 0.1f);
        }
    }
}
