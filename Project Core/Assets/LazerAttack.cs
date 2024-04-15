using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAttack : MonoBehaviour
{
    [SerializeField] private LineRenderer _Beam;
    [SerializeField] private Transform Firepoint;
    [SerializeField] private float maxlength;
    public float MaxDuration;
    private float Duration;
    public bool isLazerAttacking;
    public float trackingSpeed;
    public int LazerDamage;
    public void attack()
    {
        
        Activate();
    }
    
    private void Activate()
    {
        _Beam.enabled = true;
        
    }
    private void Deactivate()
    {
        _Beam.enabled = false;
        isLazerAttacking = false;
        _Beam.SetPosition(0, Firepoint.position);
        _Beam.SetPosition(1, Firepoint.position);
    }
    
    
    private void Update()
    {
        
        if (_Beam.enabled)
        {
            Vector3 playerPosition = Player.Instance.transform.position;
            Vector3 directionToPlayer = playerPosition - Firepoint.position;

            // Smoothly rotate the Firepoint towards the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            Firepoint.rotation = Quaternion.Lerp(Firepoint.rotation, targetRotation, Time.deltaTime * trackingSpeed);
            _Beam.SetPosition(0, Firepoint.position);
            Debug.Log(_Beam.transform.position);

            Ray ray = new Ray(Firepoint.position, Firepoint.forward);


            bool cast = Physics.Raycast(ray, out RaycastHit hit, maxlength);


            Vector3 hitPosition = Firepoint.position + Firepoint.forward * maxlength;
            if (cast)
            {
                hitPosition = hit.point;
            }
            if (cast)
            {
                hitPosition = hit.point;

                // Check if the raycast hit the player
                if (hit.collider.CompareTag("Player"))
                {
                    // Apply damage to the player
                    Player player = hit.collider.GetComponent<Player>();
                    player.DamageRecieve(LazerDamage);
                }
            }

            _Beam.SetPosition(1, hitPosition);
            if (Duration > 0)
            {
                Duration -= Time.deltaTime;
            }
            else
            {
                Deactivate();
                Duration = MaxDuration;
            }
        }

        
       
        
    }
}
