using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isOrange;
    public float distance = 0f;
    public float yOffset = 1.0f; // Adjust this value to set how high above the floor the player should spawn
    private Transform destination;
    private bool HasTeleported;

    private void Start()
    {
        if (isOrange == false)
        {
            destination = GameObject.FindGameObjectWithTag("OrangePortal").GetComponent<Transform>();
        }
        else
        {
            destination = GameObject.FindGameObjectWithTag("BluePortal").GetComponent<Transform>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasTeleported)
        {
            if (Vector3.Distance(transform.position, other.transform.position) > distance)
            {
                HasTeleported = true;
                Vector3 destinationPosition = destination.position + Vector3.up;
                other.transform.position = destinationPosition;
            }
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HasTeleported = false;
        }
    }
}
