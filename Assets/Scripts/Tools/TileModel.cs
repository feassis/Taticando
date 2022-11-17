using System;

public class TileModel
{
    public int GetCost(TileType hexType) => hexType switch
    {
        TileType.Default => 10,
        TileType.Difficult => 20,
        TileType.Road => 5,
        _ => throw new Exception($"Hex Type {hexType} Not Supported"),
    };

    public bool IsObstacle(TileType hexType)
    {
        return hexType == TileType.Obstacle || hexType == TileType.Water;
    }
}