using MVC.Controler.Combat;
using MVC.View.Unit;
using System.Collections.Generic;
using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "Apply Element On Grid", menuName = "Units/Action/Graphics/Apply Elements On Grid")]
public class ApplyElementOnUnitActionRange : UnitActionVisuals
{
    [SerializeField] private ElementsEnum element;
    public override void Execute(UnitGraphics unit)
    {
        var actionRange = unit.GetCurrentActionRange();

        var gridservice = ServiceLocator.GetService<GridService>();

        List<Vector3Int> tilesToApplyElements = new List<Vector3Int>();

        foreach (Vector3Int pos in actionRange.GetRangePositions())
        {
            tilesToApplyElements.Add(pos);
        }

        var combatManager = ServiceLocator.GetService<CombatManager>();

        var team = combatManager.GetTeamOfAUnit(unit.gameObject);

        gridservice.ApplyElementToTiles(tilesToApplyElements, element, team);
    }
}

