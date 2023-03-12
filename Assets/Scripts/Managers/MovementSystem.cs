using MVC.Controller.Combat;
using MVC.Controller.Graph;
using MVC.Model.Combat;
using MVC.View.Grid;
using MVC.View.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

namespace MVC.Controller.Movement
{
    public class MovementSystem
    {
        private BFSResult movementRange = new BFSResult();
        private List<Vector3Int> currentPath = new List<Vector3Int>();
        private int currentCost;

        public void HideRange(IGrid grid)
        {
            foreach (Vector3Int tilePosition in movementRange.GetRangePositions())
            {
                grid.GetTileAt(tilePosition).DisableHighlight();
            }

            movementRange = new BFSResult();
        }

        public void ShowRange(UnitGraphics selectedUnit, IGrid grid, TeamEnum team)
        {
            CalculateRange(selectedUnit, grid, team);

            Vector3Int unitPos = grid.GetClosestTile(selectedUnit.transform.position);

            foreach (Vector3Int position in movementRange.GetRangePositions())
            {
                if (unitPos == position)
                {
                    continue;
                }

                grid.GetTileAt(position).EnableHighlight();
            }
        }

        private void CalculateRange(UnitGraphics selectedUnit, IGrid grid, TeamEnum team)
        {
            currentPath.Clear();
            movementRange = new BFSResult();
            var graphSearchService = ServiceLocator.GetService<GraphSearch>();
            movementRange = graphSearchService.BFSGetRange(grid,
                grid.GetClosestTile(selectedUnit.transform.position), selectedUnit.MovementPoints, NeighbourhoodType.Radial, team);
        }

        public void ShowPath(Vector3Int selectedHexPosition, IGrid grid, TeamEnum team)
        {
            if (movementRange.GetRangePositions().Contains(selectedHexPosition))
            {
                foreach (Vector3Int position in currentPath)
                {
                    grid.GetTileAt(position).ResetHightlight();
                }

                (var path, var cost) = movementRange.GetPathTo(selectedHexPosition, team);

                currentPath = path;
                currentCost = cost;

                foreach (Vector3Int position in currentPath)
                {
                    grid.GetTileAt(position).HighlightPath();
                }
            }
        }

        public void MoveUnit(UnitGraphics selectedUnit, IGrid grid)
        {
            Debug.Log("Moving Unit " + selectedUnit.name);
            selectedUnit.MoveThroughPath(currentPath.Select(pos => grid.GetTileAt(pos).transform.position).ToList(), currentCost);
            selectedUnit.MovementFinished += UnitMovementFinished;
        }

        public void RotateInPlace(UnitGraphics selectedUnit, RotationOrientarition direction, Action onRotationFinished)
        {
            selectedUnit.RotateInPlace(direction, onRotationFinished);
        }

        private void UnitMovementFinished(UnitGraphics unit)
        {
            currentPath.Clear();
            movementRange = new BFSResult();
            var combatManager = ServiceLocator.GetService<CombatManager>();

            combatManager.CheckTeamHasActionsToDo();
        }

        public bool IsTileInRange(Vector3Int hexPosition)
        {
            return movementRange.IsTilePositionInRange(hexPosition);
        }
    }
}

