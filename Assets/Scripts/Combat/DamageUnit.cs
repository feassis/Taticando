using MVC.Controller.Combat;
using MVC.Model.Combat;
using MVC.View.Unit;
using Tools;
using UnityEngine;

namespace MVC.Controller.Unit
{
    [CreateAssetMenu(fileName = "Damage Unit", menuName = "Units/Action/Damage Unit")]
    public class DamageUnit : UnitAction
    {
        [SerializeField] private DamageType type;

        public override int Execute(UnitGraphics unit)
        {
            var combatManager = ServiceLocator.GetService<CombatManager>();

            float modifier = combatManager.GetUnitOfATeam(unit).GetActionModifier(actionType);

            return combatManager.DamageUnit(unit, Mathf.RoundToInt(modifier), type);
        }
    }
}


