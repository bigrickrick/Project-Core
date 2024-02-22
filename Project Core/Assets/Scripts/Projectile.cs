using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int ProjectileDamage;
    public float ProjectileSpeed;

    public abstract void ApplyEffect();
    
}