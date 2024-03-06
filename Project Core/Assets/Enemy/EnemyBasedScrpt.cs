using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class EnemyBasedScrpt : Entity
{
    public float attackRange;
    public float rotationSpeed = 100;
    public Transform EnemyCanon;
    public abstract void Attack();

    public bool CheckifPlayerInAttackRange()
    {
        
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                EnemyCanon.LookAt(Player.Instance.transform);
                return true;
            }
        }

        
        return false;
    }
    [SerializeField] private Image Hpbar;
    public Projectile EnemyProjectile;
    public Transform firepoint;
    public float baseAttackTimer;
    protected float attackTimer;
    private void Start()
    {
        attackTimer = baseAttackTimer / attackspeedModifier;
    }
    private void LateUpdate()
    {
        UpdateHpbar();
        if (CheckifPlayerInAttackRange())
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attackTimer = baseAttackTimer / attackspeedModifier;
                Attack();
            }
        }


    }
    public void UpdateHpbar()
    {
        if (Hpbar == null)
        {
            Debug.LogError(this.gameObject.name + " Hpbar is null!");
            return;
        }

        float hp = (float)HealthPoints / (float)maxHealthPoints;
        Hpbar.fillAmount = hp;
    }



}
