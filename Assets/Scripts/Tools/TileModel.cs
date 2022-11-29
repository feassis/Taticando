using System;

public class TileModel
{
    private ElementsEnum elements;
    private TileType type;

    public TileModel(TileType type)
    {
        this.type = type;
    }

    public int GetDefaultCost(TileType tileType) => tileType switch
    {
        TileType.Default => 10,
        TileType.Difficult => 20,
        TileType.Road => 5,
        _ => throw new Exception($"Hex Type {tileType} Not Supported"),
    };

    public int GetCost()
    {
        return GetDefaultCost(type);
    }

    public bool IsTileAfflictedByElement(ElementsEnum element)
    {
        return elements.HasFlag(element);
    }

    public bool IsObstacle()
    {
        return type == TileType.Obstacle || type == TileType.Water;
    }
}