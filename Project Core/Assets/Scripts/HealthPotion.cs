using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public override void Apply(GameObject target)
    {
        Player entity = target.GetComponent<Player>();
        if(entity.HealthPoints+amount > entity.maxHealthPoints)
        {
            entity.HealthPoints = entity.maxHealthPoints;
        }
        else if(entity.HealthPoints+amount < entity.maxHealthPoints)
        {
            entity.HealthPoints += amount;
        }
    }

}
