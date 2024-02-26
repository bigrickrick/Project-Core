using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maddnessBall : Potion
{
    public Madness madnessbar;


    public override void Apply(GameObject target)
    {
        madnessbar.ApplyMaddness(Player.Instance);
    }
    


    
    


}
