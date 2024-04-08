using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifiers : MonoBehaviour
{
    [Header("WhatCanAffectIt")]
    public bool CanBeIncapacited;
    public bool CanBedamage;
    public bool IsIncapacited;

    [Header("ExtraModifier")]
    public bool IsRaged;

    private void Start()
    {
        if (IsRaged)
        {
            Rage();
        }
    }
    public void SetIncapacited()
    {
        if (CanBeIncapacited)
        {
            IsIncapacited = !IsIncapacited;
        }
    }
    private void Rage()
    {
        gameObject.GetComponent<Entity>().EntitySpeed = gameObject.GetComponent<Entity>().EntitySpeed * 1.5f;
        gameObject.GetComponent<Entity>().attackspeedModifier += 1;

    }
    
    
}
