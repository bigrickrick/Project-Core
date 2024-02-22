using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : Potion
{
    public float durationInSeconds = 1f;

    public override void Apply(GameObject target)
    {
        Player entity = target.GetComponent<Player>();
        if (entity != null)
        {
            
            entity.SprintSpeed += amount;
            entity.CroutchSpeed += amount;
            entity.attackspeedModifier += amount;
            StartCoroutine(RemoveBuff(entity));
            // Don't destroy the potion here, let it be destroyed on contact.
        }
    }

    private IEnumerator RemoveBuff(Player entity)
    {
        yield return new WaitForSeconds(durationInSeconds);

       
        entity.SprintSpeed -= amount;
        entity.CroutchSpeed -= amount;
        entity.attackspeedModifier -= amount;

        // Make sure the potion still exists before attempting to destroy it
        if (gameObject != null)
            Destroy(gameObject);
    }
}
