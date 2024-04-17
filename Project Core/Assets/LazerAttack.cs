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
    public EnemyAi Enemy;
    public AudioClip LazerSoundEffect;
    private AudioSource audioSource;

    private void Start()
    {
        // Add an AudioSource component if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip and configure AudioSource properties
        audioSource.clip = LazerSoundEffect;
        audioSource.loop = true; // Loop the laser sound continuously
        audioSource.playOnAwake = false; // Don't play the sound automatically on awake
    }

    public void Attack()
    {
        Activate();
    }

    private void Activate()
    {
        if (LazerSoundEffect != null && audioSource != null)
        {
            audioSource.Play();
        }
        _Beam.enabled = true;
        isLazerAttacking = true;

        
        
    }

    private void Deactivate()
    {
        _Beam.enabled = false;
        isLazerAttacking = false;
        _Beam.SetPosition(0, Firepoint.position);
        _Beam.SetPosition(1, Firepoint.position);

        // Stop the laser sound effect
        if (audioSource != null)
        {
            audioSource.Stop();
        }
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

            Ray ray = new Ray(Firepoint.position, Firepoint.forward);

            bool cast = Physics.Raycast(ray, out RaycastHit hit, maxlength);

            Vector3 hitPosition = Firepoint.position + Firepoint.forward * maxlength;
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
