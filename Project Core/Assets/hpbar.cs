using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class hpbar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private TMPro.TextMeshProUGUI hpbartext;





    private void Update()
    {
        UpdateHpbar();
    }
    private void UpdateHpbar()
    {
        float healthPercentage = (float)Player.Instance.GetComponent<Entity>().HealthPoints / (float)Player.Instance.GetComponent<Entity>().maxHealthPoints;
        Debug.Log("Health Percentage: " + healthPercentage);
        hpBar.fillAmount = healthPercentage;
        hpbartext.text = Player.Instance.GetComponent<Entity>().HealthPoints.ToString();

    }
}
