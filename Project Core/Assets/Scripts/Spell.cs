using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellStats spell;
    public bool HasSpellInHand;
    
    public abstract void ShootSpell(Transform firepoint);

   



}
