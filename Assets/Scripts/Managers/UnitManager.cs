using MVC.Controler.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class UnitManager
{
    private bool canMoving = true;

    [SerializeField] private UnitGraphics selectedUnit;
    private TileGraphics previousSelectedTile;

    public static UnitManager Create(CombatManager combatManager)
    {
        var unitManager = new UnitManager();
        combatManager.OnTurnChange += unitManager.HideMovementRange;
        return unitManager;
    }

    public void HandleUnitSelected(GameObject unit)
    {
        if (!IsUnitsTurn(unit))
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

    public void ShowUnitActionRange()
    {
        var grid = ServiceLocator.GetService<IGrid>();
        selectedUnit.ShowActionRange(grid);
    }

    private bool IsUnitsTurn(GameObject unit)
    {
        var combatManget = ServiceLocator.GetService<CombatManager>();

        return combatManget.IsThisUnitTurn(unit);
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
        if(selectedUnit == null || canMoving == false)
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
        canMoving = false;
        selectedUnit.MovementFinished += FinishMovement;
        ClearOldSelection();
    }

    private void FinishMovement(UnitGraphics obj)
    {
        obj.MovementFinished -= FinishMovement;
        canMoving = true;
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

    public void RotateUnitInPlace(RotationOrientarition direction)
    {
        ServiceLocator.GetService<MovementSystem>().RotateInPlace(selectedUnit, direction, ShowUnitActionRange);
    }

    public void HideMovementRange()
    {
        var grid = ServiceLocator.GetService<IGrid>();
        ServiceLocator.GetService<MovementSystem>().HideRange(grid);
    }

    public void ClearOldSelection()
    {
        Debug.Log("Clear");
        var grid = ServiceLocator.GetService<IGrid>();
        previousSelectedTile = null;

        if(selectedUnit != null)
        {
            selectedUnit.Deselect();
        }
        
        ServiceLocator.GetService<MovementSystem>().HideRange(grid);
        selectedUnit = null;
    }
}
