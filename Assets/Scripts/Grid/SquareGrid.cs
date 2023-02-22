using MVC.Controller.Grid;
using MVC.View.Tile;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.View.Grid
{
    public class SquareGrid : MonoBehaviour, IGrid
    {
        Dictionary<Vector3Int, TileGraphics> tileDict = new Dictionary<Vector3Int, TileGraphics>();
        Dictionary<Vector3Int, List<Vector3Int>> tileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();

        private void Awake()
        {
            ServiceLocator.RegisterService<IGrid>(this);
        }

        public void ResetNeighbourhoodTileDicts()
        {
            tileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();
        }

        private void Start()
        {
            var gridService = ServiceLocator.GetService<GridService>();
            foreach (var tile in FindObjectsOfType<TileGraphics>())
            {
                tileDict[tile.Coords] = tile;
                gridService.AddTile(tile.Coords, tile.TileType);
            }
        }

        public TileGraphics GetTileAt(Vector3Int coordinates)
        {
            tileDict.TryGetValue(coordinates, out TileGraphics tile);
            return tile;
        }

        public List<Vector3Int> GetNeighBoursFor(Vector3Int coordinate, NeighbourhoodType type)
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

            foreach (var direction in Direction.GetDirectionList(type))
            {
                if (tileDict.ContainsKey(coordinate + direction))
                {
                    tileNeighboursDict[coordinate].Add(coordinate + direction);
                }
            }

            return tileNeighboursDict[coordinate];
        }

        public (List<Vector3Int> tempPositions, List<Vector3Int> neighbours) GetNeighBoursForForced(Vector3Int coordinate, List<Vector3Int> tempPositions, NeighbourhoodType type)
        {
            List<Vector3Int> tempPosition = new List<Vector3Int>();
            tempPosition.AddRange(tempPositions);

            if (!tileDict.ContainsKey(coordinate))
            {
                tileDict.Add(coordinate, null);

                if (!tempPosition.Contains(coordinate))
                {
                    tempPosition.Add(coordinate);
                }
                var neighbours = GetNeighBoursFor(coordinate, type);

                return (tempPosition, neighbours);
            }

            if (tileNeighboursDict.ContainsKey(coordinate))
            {
                return (tempPosition, tileNeighboursDict[coordinate]);
            }

            tileNeighboursDict.Add(coordinate, new List<Vector3Int>());

            foreach (var direction in Direction.GetDirectionList(type))
            {
                if (tileDict.ContainsKey(coordinate + direction))
                {
                    tileNeighboursDict[coordinate].Add(coordinate + direction);
                }
                else
                {
                    tileDict.Add(coordinate + direction, null);
                    tileNeighboursDict[coordinate].Add(coordinate + direction);

                    if (!tempPosition.Contains(coordinate + direction))
                    {
                        tempPosition.Add(coordinate + direction);
                    }
                }
            }

            return (tempPosition, tileNeighboursDict[coordinate]);
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

        public void RemoveTemporaryNodes(List<Vector3Int> temporaryNodes)
        {
            foreach (var node in temporaryNodes)
            {
                tileDict.Remove(node);
                tileNeighboursDict.Remove(node);
            }
        }
    }

    public interface IGrid
    {
        void ResetNeighbourhoodTileDicts();
        TileGraphics GetTileAt(Vector3Int coordinates);
        List<Vector3Int> GetNeighBoursFor(Vector3Int coordinate, NeighbourhoodType type);
        (List<Vector3Int> tempPositions, List<Vector3Int> neighbours) GetNeighBoursForForced(Vector3Int coordinate, List<Vector3Int> tempPositions, NeighbourhoodType type);
        Vector3Int GetClosestTile(Vector3 worldPosition);
        void RemoveTemporaryNodes(List<Vector3Int> temporaryNodes);
    }
}

