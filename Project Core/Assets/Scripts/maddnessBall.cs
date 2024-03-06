using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maddnessBall : Potion
{
    public Madness madnessbar;
    public Spell spellTogive1;
    public Spell spellTogive2;

    public override void Apply(GameObject target)
    {
        madnessbar.ApplyMaddness(Player.Instance);
        Player.Instance.spellInventory.AddSpellToSpellLists(spellTogive1,spellTogive2);
    }
    


    
    


}
