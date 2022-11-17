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
}
