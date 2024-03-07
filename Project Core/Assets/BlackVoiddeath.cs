using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackVoiddeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Entity>() == true)
        {
            other.GetComponent<Entity>().DamageRecieve(999999);
        }
        
    }
}
