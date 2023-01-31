using MVC.Controler.Combat;
using MVC.View.Unit;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


[CreateAssetMenu(fileName = "Element Application Config", menuName = "Configs/Elements/ApplicationConfig")]
public class ElementApplicationConfig : ScriptableObject
{
    [SerializeField] private List<ElementApplicationAction> actionsOnElementApplication;

    public void ActionOnApplication(UnitGraphics unit, ElementsEnum element)
    {
        ElementApplicationAction elementAction = actionsOnElementApplication.Find(e => e.Element == element);

        if(elementAction == null)
        {
            return;
        }

        elementAction.ExecuteElementApplicationActions(unit);
    }
}

[Serializable]
public class ElementApplicationAction
{
    public ElementsEnum Element;
    public List<ActionByTeam> ActionsByTeam; 

    public int ExecuteElementApplicationActions(UnitGraphics unit)
    {
        var team = ServiceLocator.GetService<CombatManager>().GetTeamOfAUnit(unit);
        var teamAction = ActionsByTeam.Find(a => a.Team == team);

        if(teamAction == null)
        {
            return -1;
        }

        return teamAction.ExecuteActions(unit);
    }
}

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
