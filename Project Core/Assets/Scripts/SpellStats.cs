using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewSpellStats", menuName = "Spell Stats")]
public class SpellStats : ScriptableObject
{
    public string spellName;
    public int damage;
    public float castTime;
    public GameObject spellProjectile;
}
