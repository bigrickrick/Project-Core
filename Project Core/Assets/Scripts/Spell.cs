using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellStats spell;
    public bool HasSpellInHand;
    public abstract void ShootSpell(Transform firepoint);

    public void PLaySpellSound()
    {
        if (spell.SpellOnshootSound != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = spell.SpellOnshootSound;
            audioSource.Play();
            Destroy(audioSource, spell.SpellOnshootSound.length + 0.1f);
        }
    }



}
