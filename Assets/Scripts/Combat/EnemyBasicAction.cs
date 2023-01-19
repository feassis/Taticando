using MVC.Controler.Combat;
using MVC.View.Unit;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class EnemyBasicAction : UnitAction
{
    public override int Execute(UnitGraphics unit)
    {
        return 0;
    }

    protected List<UnitInCombat> GetPossibleTargets(UnitGraphics unit)
    {
        var actionRange = unit.GetCurrentActionRange();

        var gridservice = ServiceLocator.GetService<GridService>();

        List<Vector3Int> tilesToApplyElements = new List<Vector3Int>();

        List<UnitInCombat> possibleTargets = new List<UnitInCombat>();

        foreach (Vector3Int pos in actionRange.GetRangePositions())
        {
            tilesToApplyElements.Add(pos);
        }

        return ServiceLocator.GetService<CombatManager>().GetUnitsOnRange(tilesToApplyElements);
    }
}
