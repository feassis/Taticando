using MVC.Controller.Combat;
using MVC.View.Unit;
using Tools;
using UnityEngine;

namespace MVC.Controller.Unit
{
    [CreateAssetMenu(fileName = "Gain Shield", menuName = "Units/Action/Gain Shied")]
    public class GainShield : UnitAction
    {
        public override int Execute(UnitGraphics unit)
        {
            var combatManager = ServiceLocator.GetService<CombatManager>();

            float amountToGain= combatManager.GetUnitOfATeam(unit).GetActionModifier(actionType);

            combatManager.GiveShield(unit, Mathf.RoundToInt(amountToGain));

            return 0;
        }
    }
}


