using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int ProjectileDamage;
    public float ProjectileSpeed;
    public float currentVelocity;
    public abstract void ApplyEffect();
    public WhichBullet whichBullet;
    public string Target;
    public enum WhichBullet
    {
        Player,
        Enemy,
    }
    private void Start()
    {
        
        if (whichBullet == WhichBullet.Player)
        {
            Target = "Enemy";
        }
        else
        {
            Target = "Player";
        }
    }
}
