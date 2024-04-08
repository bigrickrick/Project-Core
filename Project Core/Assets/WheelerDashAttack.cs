using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class WheelerDashAttack : EnemyAttacks
{
    public Rigidbody rb;
    public ParticleSystem sparksParticleSystem;
    public float dashForce; 
    public float dashDuration = 0.5f; // Duration of each dash
    public float dashInterval = 0.5f; // Interval between dashes
    private void Start()
    {
        sparksParticleSystem.Stop();
    }
    // Define the directions for a square dash
    private Vector3[] squareDirections = {
        Vector3.forward,
        Vector3.right,
        Vector3.back,
        Vector3.left
    };
    private int currentDirectionIndex = 0;

    public override void attack()
    {
        StartCoroutine(DoDash());
    }

    IEnumerator DoDash()
    {
        
        for (int i = 0; i < squareDirections.Length; i++)
        {
            Vector3 dashDirection = squareDirections[currentDirectionIndex];
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);

            Quaternion targetRotation = Quaternion.LookRotation(-dashDirection);
            sparksParticleSystem.transform.rotation = targetRotation;
            transform.LookAt(transform.position + dashDirection);

            
            sparksParticleSystem.Play();

            currentDirectionIndex = (currentDirectionIndex + 1) % squareDirections.Length;

            yield return new WaitForSeconds(dashDuration);

            yield return new WaitForSeconds(dashInterval);
        }
        
        sparksParticleSystem.Stop();
    }


}
