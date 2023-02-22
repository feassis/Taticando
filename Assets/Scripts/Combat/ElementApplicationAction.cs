using MVC.Controller.Combat;
using MVC.Model.Elements;
using MVC.View.Unit;
using System;
using System.Collections.Generic;
using Tools;


namespace MVC.Controller.Unit
{
    [Serializable]
    public class ElementApplicationAction
    {
        public ElementsEnum Element;
        public List<ActionByTeam> ActionsByTeam;

        public int ExecuteElementApplicationActions(UnitGraphics unit)
        {
            var team = ServiceLocator.GetService<CombatManager>().GetTeamOfAUnit(unit);
            var teamAction = ActionsByTeam.Find(a => a.Team == team);

            if (teamAction == null)
            {
                return -1;
            }

            return teamAction.ExecuteActions(unit);
        }
    }
}


