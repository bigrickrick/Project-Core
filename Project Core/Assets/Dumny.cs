using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dumny : Entity
{
    [SerializeField] private Image Hpbar;

    private void LateUpdate()
    {
        UpdateHpbar();
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

}
