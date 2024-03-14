using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maddnessBall : Potion
{
    public Madness madnessbar;
    public Element elementTogiveleft;
    public Element elementTogiveright;

    public override void Apply(GameObject target)
    {
        madnessbar.ApplyMaddness(Player.Instance);
        Player.Instance.spellInventory.AddSpellToSpellLists(elementTogiveleft,elementTogiveright);
    }
    


    
    


}
