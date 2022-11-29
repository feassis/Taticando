using System.Collections.Generic;
using UnityEngine;

public class GridService
{
    Dictionary<Vector3Int, TileModel> tileDict = new Dictionary<Vector3Int, TileModel>();

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

    public int GetTileCost(Vector3Int position)
    {
        return tileDict[position].GetCost();
    }

    public bool IsTileAnObstacle(Vector3Int position)
    {
        return tileDict[position].IsObstacle(); 
    }
}
