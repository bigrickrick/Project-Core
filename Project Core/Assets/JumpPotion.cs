using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPotion : Potion
{
    public float durationInSeconds = 1f;

    public override void Apply(GameObject target)
    {
        Player entity = target.GetComponent<Player>();
        if (entity != null)
        {
            entity.changeJumpForce(amount);

            StartCoroutine(RemoveBuff(entity));
            
        }
    }

    private IEnumerator RemoveBuff(Player entity)
    {
        yield return new WaitForSeconds(durationInSeconds);

        entity.ResetJumpForce();
        if (gameObject != null)
            Destroy(gameObject);
    }
}
