using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools;

public class GraphSearch
{
    public BFSResult BFSRangeAllCosts1(IGrid grid, Vector3Int startPoint, int movementPoints, NeighbourhoodType type, bool forcedNeibourhood = false)
    {
        Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
        Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        var gridService = ServiceLocator.GetService<GridService>();

        List<Vector3Int> tempNodes = new List<Vector3Int>();

        while (nodesToVisitQueue.Count > 0)
        {
            Vector3Int currentNode = nodesToVisitQueue.Dequeue();

            List<Vector3Int> neighbours = new List<Vector3Int>();

            if (forcedNeibourhood)
            {
                var neighboursFoced = grid.GetNeighBoursForForced(currentNode, tempNodes, type);
                tempNodes = neighboursFoced.tempPositions;
                neighbours = neighboursFoced.neighbours;
            }
            else
            {
                neighbours = grid.GetNeighBoursFor(currentNode, type);
            }

            foreach (Vector3Int neighbourPosition in neighbours)
            {
                if (!forcedNeibourhood && gridService.IsTileAnObstacle(neighbourPosition))
                {
                    continue;
                }

                int nodeCost = 1;
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                if (newCost > movementPoints)
                {
                    continue;
                }

                if (!visitedNodes.ContainsKey(neighbourPosition))
                {
                    visitedNodes[neighbourPosition] = currentNode;
                    costSoFar[neighbourPosition] = newCost;
                    nodesToVisitQueue.Enqueue(neighbourPosition);
                }
                else if (costSoFar[neighbourPosition] > newCost)
                {
                    costSoFar[neighbourPosition] = newCost;
                    visitedNodes[neighbourPosition] = currentNode;
                }
            }
        }

        grid.RemoveTemporaryNodes(tempNodes);

        return new BFSResult { visitedNodesDict = visitedNodes };
    }

    public BFSResult BFSGetRange(IGrid grid, Vector3Int startPoint, int movementPoints, NeighbourhoodType type)
    {
        Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
        Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint, null);

        var gridService = ServiceLocator.GetService<GridService>();

        while (nodesToVisitQueue.Count > 0)
        {
            Vector3Int currentNode = nodesToVisitQueue.Dequeue();

            foreach (Vector3Int neighbourPosition in grid.GetNeighBoursFor(currentNode, type))
            {
                if (gridService.IsTileAnObstacle(neighbourPosition))
                {
                    continue;
                }

                int nodeCost = gridService.GetTileCost(neighbourPosition);
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                if (newCost > movementPoints)
                {
                    continue;
                }

                if (!visitedNodes.ContainsKey(neighbourPosition))
                {
                    visitedNodes[neighbourPosition] = currentNode;
                    costSoFar[neighbourPosition] = newCost;
                    nodesToVisitQueue.Enqueue(neighbourPosition);
                }
                else if (costSoFar[neighbourPosition] > newCost)
                {
                    costSoFar[neighbourPosition] = newCost;
                    visitedNodes[neighbourPosition] = currentNode;
                }
            }
        }

        return new BFSResult { visitedNodesDict = visitedNodes };
    }

    internal List<Vector3Int> GeneratePathPFS(Vector3Int current, Dictionary<Vector3Int, Vector3Int?> visitedNodesDict)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(current);

        while (visitedNodesDict[current] != null)
        {
            path.Add(visitedNodesDict[current].Value);
            current = visitedNodesDict[current].Value;
        }

        path.Reverse();
        return path.Skip(1).ToList();
    }
}

public class BFSResult
{
    public Dictionary<Vector3Int, Vector3Int?> visitedNodesDict;

    public (List<Vector3Int> path, int pathCost) GetPathTo(Vector3Int destination)
    {
        if (!visitedNodesDict.ContainsKey(destination))
        {
            return (new List<Vector3Int>(), 0);
        }

        var graphSearch = ServiceLocator.GetService<GraphSearch>();
        var grid = ServiceLocator.GetService<IGrid>();
        var gridService = ServiceLocator.GetService<GridService>();

        var path = graphSearch.GeneratePathPFS(destination, visitedNodesDict);

        int cost = 0;

        foreach (var node in path)
        {
            cost += gridService.GetTileCost(node);
        }

        return (path, cost);
    }

    public bool IsTilePositionInRange(Vector3Int position)
    {
        return visitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetRangePositions() => visitedNodesDict == null ? new List<Vector3Int>() : visitedNodesDict.Keys;
}
