using System;
using System.Collections.Generic;
using UnityEngine;

public class GridService
{
    Dictionary<Vector3Int, TileModel> tileDict = new Dictionary<Vector3Int, TileModel>();

    public Action OnElementApplied;

    public void Setup()
    {
        
    }

    public void AddTile(Vector3Int position, TileType type)
    {
        if (tileDict.ContainsKey(position))
        {
            Debug.LogError($"already exists tile at {position}");
            return;
        }

        tileDict.Add(position, new TileModel(type));
    }

    public void ApplyElementToTiles(List<Vector3Int> positions, ElementsEnum element)
    {
        foreach (var pos in positions)
        {
            if(tileDict[pos] == null)
            {
                continue;
            }

            tileDict[pos].ApplyElement(element);
        }

        OnElementApplied?.Invoke();
    }

    public bool IsTileAfflictedByElement(Vector3Int position, ElementsEnum element)
    {
        return tileDict[position].IsTileAfflictedByElement(element);
    }

    public int GetTileCost(Vector3Int position, TeamEnum team)
    {
        return tileDict[position].GetCost(team);
    }

    public bool IsTileAnObstacle(Vector3Int position)
    {
        return tileDict[position].IsObstacle(); 
    }
}
