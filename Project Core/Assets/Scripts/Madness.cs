using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Madness : MonoBehaviour
{
    
    public int maddnesslevelmax;

    private int maddnesslevel = 0;

    public float maddnessspeedincrease= 0.1f;

    public float maddnessAttackspeedincrease=0.1f;
    
    public void ApplyMaddness(Player player)
    {
        if(maddnesslevel <maddnesslevelmax)
        {
            player.attackspeedModifier += maddnessAttackspeedincrease;
            player.EntitySpeed += maddnessspeedincrease;
           
            player.SprintSpeed += maddnessspeedincrease;
            player.CroutchSpeed += maddnessspeedincrease;
            player.dashSpeed+= maddnessspeedincrease;
            maddnesslevel += 1;
        }
        
        
    }

    [SerializeField] private Image maddnessbar;
    





    private void Update()
    {
        UpdateHpbar();
    }
    private void UpdateHpbar()
    {
        float maddness = (float)maddnesslevel / (float)maddnesslevelmax;
        Debug.Log("Health Percentage: " + maddness);
        maddnessbar.fillAmount = maddness;
        

    }


}
