using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

public class MovementSystem
{
    private BFSResult movementRange = new BFSResult();
    private List<Vector3Int> currentPath = new List<Vector3Int>();

    public void HideRange(IGrid grid)
    {
        foreach (Vector3Int tilePosition in movementRange.GetRangePositions())
        {
            grid.GetTileAt(tilePosition).DisableHighlight();
        }

        movementRange = new BFSResult();
    }

    public void ShowRange(UnitGraphics selectedUnit, IGrid grid)
    {
        CalculateRange(selectedUnit, grid);

        Vector3Int unitPos = grid.GetClosestTile(selectedUnit.transform.position);

        foreach (Vector3Int position in movementRange.GetRangePositions())
        {
            if(unitPos == position)
            {
                continue;
            }

            grid.GetTileAt(position).EnableHighlight();
        }
    }

    private void CalculateRange(UnitGraphics selectedUnit, IGrid grid)
    {
        var graphSearchService = ServiceLocator.GetService<GraphSearch>();
        movementRange = graphSearchService.BFSGetRange(grid,
            grid.GetClosestTile(selectedUnit.transform.position), selectedUnit.MovementPoints);
    }

    public void ShowPath(Vector3Int selectedHexPosition, IGrid grid)
    {
        if (movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            foreach (Vector3Int position in currentPath)
            {
                grid.GetTileAt(position).ResetHightlight();
            }

            currentPath = movementRange.GetPathTo(selectedHexPosition);

            foreach (Vector3Int position in currentPath)
            {
                grid.GetTileAt(position).HighlightPath();
            }
        }
    }

    public void MoveUnit(UnitGraphics selectedUnit, IGrid grid)
    {
        Debug.Log("Moving Unit " + selectedUnit.name);
        selectedUnit.MoveThroughPath(currentPath.Select(pos => grid.GetTileAt(pos).transform.position).ToList());
    }

    public bool IsTileInRange(Vector3Int hexPosition)
    {
        return movementRange.IsHexPositionInRange(hexPosition);
    }
}
