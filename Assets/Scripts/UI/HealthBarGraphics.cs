using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarGraphics : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
    [SerializeField] private float reduceSpeed;

    private float targetFillAmount;

    public void Setup(int currentHp, int maxHp)
    {
        targetFillAmount = (float)currentHp / (float)maxHp;
        healthbarSprite.fillAmount = targetFillAmount;
    }
    
    public void UpdateHealthbar(int currentHp, int maxHp)
    {
        targetFillAmount = (float)currentHp / (float)maxHp;
    }

    private void Update()
    {
        healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, targetFillAmount, reduceSpeed * Time.deltaTime);
    }

    public void RequestDestruction()
    {
        Destroy(this);
    }
}