using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maddnessBall : MonoBehaviour
{
    public Madness madnessbar;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                madnessbar.ApplyMaddness(player);
                Destroy(gameObject);
            }
        }
    }


}
