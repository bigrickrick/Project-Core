using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public ParticleSystem ExplosionParticles;
    private Vector3 explosionPoint;
    public float explosionRadius = 5f;
    public int explosionDamage = 50;

    // Additional properties for tracking
    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;
    private Rigidbody targetTofollow;
    private void Start()
    {
        _speed = ProjectileSpeed;
    }
    private void FixedUpdate()
    {
        if(whichBullet == WhichBullet.Enemy)
        {
            targetTofollow = Player.Instance.GetComponent<Rigidbody>();
        }
        else
        {
            targetTofollow = FindClosestEnemy();
        }
        GetComponent<Rigidbody>().velocity = transform.forward * _speed;

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, targetTofollow.transform.position));

        PredictMovement(leadTimePercentage);

        AddDeviation(leadTimePercentage);

        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = targetTofollow.position + targetTofollow.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }
    private Rigidbody FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            return null; // No enemy found
        }

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        if (closestEnemy != null)
        {
            return closestEnemy.GetComponent<Rigidbody>(); // Return the Rigidbody component of the closest enemy
        }

        return null; // Return null if no closest enemy is found (shouldn't happen if there are enemies)
    }


    public override void ApplyEffect()
    {
        var explosionInstance = Instantiate(ExplosionParticles, explosionPoint, Quaternion.identity);
        explosionInstance.transform.LookAt(Camera.main.transform);
        Destroy(explosionInstance.gameObject, explosionInstance.main.duration);
        DamageNearbyEnemies();
    }

    private void DamageNearbyEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(explosionPoint, explosionRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(Target))
            {
                Entity entity = hitCollider.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.DamageRecieve(explosionDamage);
                    Debug.Log("Damaging " + Target + ": " + explosionDamage);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        explosionPoint = collision.contacts[0].point;
        ApplyEffect();
        Destroy(gameObject);
    }

}
