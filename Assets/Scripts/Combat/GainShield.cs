using MVC.Controler.Combat;
using MVC.View.Unit;
using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "Gain Shield", menuName = "Units/Action/Gain Shied")]
public class GainShield : UnitAction
{
    [SerializeField] private int amountToGain;

    public override int Execute(UnitGraphics unit)
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();

        combatManager.GiveShield(unit, amountToGain);

        return 0;
    }
}
