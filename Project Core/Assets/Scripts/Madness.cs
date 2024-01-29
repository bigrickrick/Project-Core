using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Madness : MonoBehaviour
{
    
    public int maddnesslevelmax;

    public float maddnessspeedincrease= 0.1f;

    public float maddnessAttackspeedincrease=0.1f;
    
    public void ApplyMaddness(Player player)
    {
        player.attackspeedModifier += maddnessAttackspeedincrease;
        player.EntitySpeed += maddnessspeedincrease;
        player.WalkSpeed+= maddnessspeedincrease;
        player.SprintSpeed += maddnessspeedincrease;
        player.CroutchSpeed += maddnessspeedincrease;
        
    }
    

}
