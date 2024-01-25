using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Madness : MonoBehaviour
{
    [SerializeField] private List<Entity> entitylist;
    public int maddnesslevel;

    public float maddnessspeedincrease= 0.1f;

    public float maddnessAttackspeedincrease=0.1f;

    public float maddnessspeedBonus;

    public float maddnessAttackspeedBonus;
    
    
    private void addMaddnesstoEntities()
    {
        //will add the madnessBonus to all entities
    }

}
