using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHolderGraphics : MonoBehaviour
{
    [SerializeField] private HealthBarGraphics shieldPrefab;
    [SerializeField] private int shieldMaxHP;
    [SerializeField] private Transform shieldHolder;

    private List<HealthBarGraphics> liveShields = new List<HealthBarGraphics>();

    public void DisplayShield(int shieldAmount)
    {
        foreach (var shield in liveShields)
        {
            shield.RequestDestruction();
        }

        liveShields.Clear();

        int shieldAmountToDisplay = shieldAmount;

        while(shieldAmountToDisplay > 0)
        {
            var shield = Instantiate(shieldPrefab, shieldHolder);
            if (shieldAmountToDisplay > shieldMaxHP)
            {
                shield.Setup(shieldMaxHP, shieldMaxHP);

                shieldAmountToDisplay -= shieldMaxHP;
            }
            else
            {
                shield.Setup(shieldAmountToDisplay, shieldMaxHP);
                shieldAmountToDisplay = 0;
            }
        }
    }

}
