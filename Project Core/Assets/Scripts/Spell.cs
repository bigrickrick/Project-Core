using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellStats spell;
    public bool HasSpellInHand;
    public SpellElement element;
    public enum SpellElement
    {
        Fire,
        space,
        Lighthing,
        earth,
        time,
    }
    public abstract void ShootSpell(Transform firepoint);

   



}
