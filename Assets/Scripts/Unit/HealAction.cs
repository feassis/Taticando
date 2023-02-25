using MVC.Controller.Combat;
using MVC.View.Unit;
using Tools;
using UnityEngine;

namespace MVC.Controller.Unit
{
    [CreateAssetMenu(fileName = "Heal Unit", menuName = "Units/Action/Heal Unit")]
    public class HealAction : UnitAction
    {
        public override int Execute(UnitGraphics unit)
        {
            var combatManager = ServiceLocator.GetService<CombatManager>();

            var healAmount = combatManager.GetUnitOfATeam(unit).GetActionModifier(actionType);

            return combatManager.HealUnit(unit, Mathf.RoundToInt(healAmount));
        }
    }
}



