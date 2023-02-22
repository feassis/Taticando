using MVC.Model.Combat;
using MVC.View.Unit;
using System.Collections.Generic;

namespace MVC.Controller.Combat
{
    public class Team
    {
        public TeamEnum MyTeam;
        public List<UnitInCombat> UnitsInCombat = new List<UnitInCombat>();

        public bool HasActionsToDo()
        {
            foreach (var unit in UnitsInCombat)
            {
                if (unit.UnitData.HasActionsToDo())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsUnitOnTeam(UnitGraphics unit)
        {
            foreach (var unitInCombat in UnitsInCombat)
            {
                if (unitInCombat.UnitOnScene.Equals(unit))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
