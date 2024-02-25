using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DashBar : MonoBehaviour
{
    [SerializeField] private Image Bar1;
    [SerializeField] private Image Bar2;
    [SerializeField] private Image Bar3;

    [SerializeField] private Dash Playerdash;
    private Image currentbar;
    private float currentDashTimer; 
    private float initialDashTimer; 

    private void Start()
    {
        
        initialDashTimer = Playerdash.dashCd;
        currentDashTimer = initialDashTimer;
        currentbar = Bar3;
    }

    private void Update()
    {
        if(Playerdash.dashCharges<3)
        {
            if(Playerdash.dashCharges == 2)
            {
                MoveToBar(Bar3);
            }
            else if(Playerdash.dashCharges == 1)
            {
                MoveToBar(Bar2);
            }
            else if(Playerdash.dashCharges <= 0)
            {
                MoveToBar(Bar1);
            }
            currentDashTimer -= Time.deltaTime;

            UpdateBarFill(currentbar);


            if (currentDashTimer <= 0)
            {
                currentDashTimer = initialDashTimer;
                MoveToNextBar();
            }
        }
        
    }

    private void UpdateBarFill(Image bar)
    {
        
        float fillAmount = Mathf.Clamp01(1 - (currentDashTimer / initialDashTimer));

        
        bar.fillAmount = fillAmount;
        
    }
    private void MoveToBar(Image bar)
    {
        float fillAmount = currentbar.fillAmount;
        currentbar.fillAmount = 0;
        currentbar = bar;
        currentbar.fillAmount = fillAmount;
    }

    private void MoveToNextBar()
    {
        
        

        
        if (currentbar == Bar1)
        {
            currentbar = Bar2;
        }
        else if (currentbar == Bar2)
        {
            currentbar = Bar3;
        }
        else if (currentbar == Bar3)
        {
            
            currentbar = Bar1;
        }
    }
}
