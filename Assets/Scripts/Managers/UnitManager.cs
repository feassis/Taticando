using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class UnitManager
{
    public bool PlayerTurn { get; private set; } = true; //this will probably change to a team enum

    [SerializeField] private UnitGraphics selectedUnit;
    private TileGraphics previousSelectedTile;

    public void HandleUnitSelected(GameObject unit)
    {
        if (PlayerTurn == false)
        {
            return;
        }

        UnitGraphics unitReference = unit.GetComponent<UnitGraphics>();

        if (CheckIfTheSameUnitSelected(unitReference))
        {
            return;
        }

        PrepareUnitForMovement(unitReference);
    }

    private void PrepareUnitForMovement(UnitGraphics unitReference)
    {
        if(selectedUnit != null)
        {
            ClearOldSelection();
        }

        selectedUnit = unitReference;

        selectedUnit.Select();

        var grid = ServiceLocator.GetService<IGrid>();

        ServiceLocator.GetService<MovementSystem>().ShowRange(selectedUnit, grid);
    }

    public void HandleTerrainSelected(GameObject tile)
    {
        if(selectedUnit == null || PlayerTurn == false)
        {
            return;
        }

        TileGraphics selectedTile = tile.GetComponent<TileGraphics>();

        if(HandleTileOutOfRange(selectedTile.Coords) || HandleSelectedTileIsUnitTile(selectedTile.Coords))
        {
            return;
        }

        HandleTargetTileSelected(selectedTile);
    }

    private void HandleTargetTileSelected(TileGraphics selectedTile)
    {
        var movementSystem = ServiceLocator.GetService<MovementSystem>();
        var grid = ServiceLocator.GetService<IGrid>();
        if (previousSelectedTile == null || previousSelectedTile != selectedTile)
        {
            previousSelectedTile = selectedTile;
            movementSystem.ShowPath(selectedTile.Coords, grid);
            return;
        }

        movementSystem.MoveUnit(selectedUnit, grid);
        PlayerTurn = false;
        selectedUnit.MovementFinished += ResetTurn;
        ClearOldSelection();
    }

    private void ResetTurn(UnitGraphics obj)
    {
        obj.MovementFinished -= ResetTurn;
        PlayerTurn = true;
    }

    private bool HandleSelectedTileIsUnitTile(Vector3Int coords)
    {
        var grid = ServiceLocator.GetService<IGrid>();
        if (coords == grid.GetClosestTile(selectedUnit.transform.position))
        {
            selectedUnit.Deselect();
            ClearOldSelection();
            return true;
        }

        return false;
    }

    private bool HandleTileOutOfRange(Vector3Int coords)
    {
        var movementSystem = ServiceLocator.GetService<MovementSystem>();

        if(movementSystem.IsTileInRange(coords) == false)
        {
            Debug.Log("Tile out of range");
            return true;
        }

        return false;
    }

    private bool CheckIfTheSameUnitSelected(UnitGraphics unitReference)
    {
        if(selectedUnit == unitReference)
        {
            ClearOldSelection();
            return true;
        }

        return false;
    }

    private void ClearOldSelection()
    {
        var grid = ServiceLocator.GetService<IGrid>();
        previousSelectedTile = null;
        selectedUnit.Deselect();
        ServiceLocator.GetService<MovementSystem>().HideRange(grid);
        selectedUnit = null;
    }
}
