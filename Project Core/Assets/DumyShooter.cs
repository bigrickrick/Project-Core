using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DumyShooter : Entity
{
    [SerializeField] private Image Hpbar;
    [SerializeField] private Projectile DummyProjectile;
    public Transform firepoint;
    public float baseAttackTimer;
    private float attackTimer;
    private void Start()
    {
        attackTimer = baseAttackTimer / attackspeedModifier;
    }
    private void LateUpdate()
    {
        UpdateHpbar();
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            attackTimer = baseAttackTimer / attackspeedModifier;
            Shoot();
        }
    }
    private void UpdateHpbar()
    {
        if (Hpbar == null)
        {
            Debug.LogError(this.gameObject.name + " Hpbar is null!");
            return;
        }

        float hp = (float)HealthPoints / (float)maxHealthPoints;
        Hpbar.fillAmount = hp;
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(DummyProjectile.gameObject, firepoint.position, firepoint.rotation);

        projectile.GetComponent<Rigidbody>().velocity = firepoint.forward.normalized * DummyProjectile.ProjectileSpeed;
    }
}
