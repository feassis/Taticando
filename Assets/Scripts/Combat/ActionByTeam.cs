using MVC.Controller.Combat;
using MVC.Model.Combat;
using MVC.View.Unit;
using System;
using System.Collections.Generic;
using Tools;

namespace MVC.Controller.Unit
{
    [Serializable]
    public class ActionByTeam
    {
        public TeamEnum Team;
        public List<UnitAction> Actions;

        public int ExecuteActions(UnitGraphics unit)
        {
            int actionsAcumulatedValue = 0;

            foreach (var action in Actions)
            {
                actionsAcumulatedValue += action.Execute(unit);
            }

            return actionsAcumulatedValue;
        }
    }
}

