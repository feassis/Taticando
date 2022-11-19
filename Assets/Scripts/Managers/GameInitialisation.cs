using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class GameInitialisation : MonoBehaviour
{
    private void Awake()
    {
        InitializeGridService();
        InitializingGraphSearch();
        InitializingMovementSystem();
        InitilizingUnitManager();
    }

    private void InitializeGridService()
    {
        var gridService = new GridService();
        gridService.Setup();

        ServiceLocator.RegisterService<GridService>(gridService);
    }

    private void InitializingGraphSearch()
    {
        var graphSearch = new GraphSearch();
        ServiceLocator.RegisterService<GraphSearch>(graphSearch);
    }

    private void InitializingMovementSystem()
    {
        var movementSystem = new MovementSystem();
        ServiceLocator.RegisterService<MovementSystem>(movementSystem);
    }

    private void InitilizingUnitManager()
    {
        var unitManager = new UnitManager();
        ServiceLocator.RegisterService<UnitManager>(unitManager);
    }
}
