using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class SquareGrid : MonoBehaviour, IGrid
{
    Dictionary<Vector3Int, TileGraphics> tileDict = new Dictionary<Vector3Int, TileGraphics>();
    Dictionary<Vector3Int, List<Vector3Int>> tileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    private void Awake()
    {
        ServiceLocator.RegisterService<IGrid>(this);
    }

    private void Start()
    {
        foreach (var tile in FindObjectsOfType<TileGraphics>())
        {
            tileDict[tile.Coords] = tile;
        }
    }

    public TileGraphics GetTileAt(Vector3Int coordinates)
    {
        tileDict.TryGetValue(coordinates, out TileGraphics tile);
        return tile;
    }

    public List<Vector3Int> GetNeighBoursFor(Vector3Int coordinate)
    {
        if (!tileDict.ContainsKey(coordinate))
        {
            return new List<Vector3Int>();
        }

        if (tileNeighboursDict.ContainsKey(coordinate))
        {
            return tileNeighboursDict[coordinate];
        }

        tileNeighboursDict.Add(coordinate, new List<Vector3Int>());

        foreach (var direction in Direction.GetDirectionList(coordinate.z))
        {
            if (tileDict.ContainsKey(coordinate + direction))
            {
                tileNeighboursDict[coordinate].Add(coordinate + direction);
            }
        }

        return tileNeighboursDict[coordinate];
    }

    private void OnDestroy()
    {
        ServiceLocator.DeregisterService<IGrid>();
    }

    public Vector3Int GetClosestTile(Vector3 worldPosition)
    {
        worldPosition.y = 0;
        return GridCoordinates.ConversPositionToOffset(worldPosition);
    }
}

public static class Direction
{
    public static List<Vector3Int> DirectionCrossNeighbourhood= new List<Vector3Int>
    {
        new Vector3Int(1,0,0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

    public static List<Vector3Int> GetDirectionList(int z) => DirectionCrossNeighbourhood;
}

public interface IGrid
{
   TileGraphics GetTileAt(Vector3Int coordinates);
   List<Vector3Int> GetNeighBoursFor(Vector3Int coordinate);
    Vector3Int GetClosestTile(Vector3 worldPosition);
}
