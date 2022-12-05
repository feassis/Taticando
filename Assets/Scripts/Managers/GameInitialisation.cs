using MVC.Controler.Combat;
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
        CombatManagerInitialization();
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

    private void InitilizingUnitManager(CombatManager combatManager)
    {
        var unitManager = UnitManager.Create(combatManager);
        ServiceLocator.RegisterService<UnitManager>(unitManager);
    }

    private void CombatManagerInitialization() //this will change once Screem Manager is implementend
    {
        var combatManager = new CombatManager();
        ServiceLocator.RegisterService<CombatManager>(combatManager);
        InitilizingUnitManager(combatManager);
    }
}
