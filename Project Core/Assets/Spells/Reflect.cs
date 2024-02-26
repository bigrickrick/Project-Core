using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect : Spell
{
    
    public override void ShootSpell(Transform firepoint)
    {
        Instantiate(spell.spellProjectile, firepoint.position,firepoint.rotation);
    }

    private void Update()
    {
        spell.spellProjectile.transform.position = Player.Instance.transform.forward;
    }
}
