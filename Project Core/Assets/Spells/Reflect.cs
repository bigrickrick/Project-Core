using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : Spell
{
    [SerializeField] private ReflecProjectile parrySheild;
    public override void ShootSpell(Transform firepoint)
    {
        parrySheild.gameObject.SetActive(true);
    }

    
}
