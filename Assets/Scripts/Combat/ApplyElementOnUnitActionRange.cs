﻿using MVC.Controller.Combat;
using MVC.Controller.Grid;
using MVC.Model.Elements;
using MVC.View.Unit;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.Controller.Unit
{
    [CreateAssetMenu(fileName = "Apply Element On Grid", menuName = "Units/Action/Graphics/Apply Elements On Grid")]
    public class ApplyElementOnUnitActionRange : UnitAction
    {
        [SerializeField] private ElementsEnum element;

        [SerializeField]
        private bool spreadElementsOnTile;
        public override int Execute(UnitGraphics unit)
        {
            var actionRange = unit.GetCurrentActionRange();

            var gridservice = ServiceLocator.GetService<GridService>();

            List<Vector3Int> tilesToApplyElements = new List<Vector3Int>();

            foreach (Vector3Int pos in actionRange.GetRangePositions())
            {
                tilesToApplyElements.Add(pos);
            }

            var combatManager = ServiceLocator.GetService<CombatManager>();

            var team = combatManager.GetTeamOfAUnit(unit);

            combatManager.ApplyElementOnUnits(tilesToApplyElements, element);

            if (spreadElementsOnTile)
            {
                gridservice.ApplyElementToTiles(tilesToApplyElements, element);
            }

            return 0;
        }
    }
}



