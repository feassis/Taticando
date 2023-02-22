using System.Collections.Generic;
using UnityEngine;
using Tools;
using MVC.Controller.Grid;
using MVC.View.Grid;
using MVC.Model.Combat;

namespace MVC.Controller.Graph
{
    public class BFSResult
    {
        public Dictionary<Vector3Int, Vector3Int?> visitedNodesDict;
        private TeamEnum team;

        public (List<Vector3Int> path, int pathCost) GetPathTo(Vector3Int destination, TeamEnum team)
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
                cost += gridService.GetTileCost(node, team);
            }

            return (path, cost);
        }

        public bool IsTilePositionInRange(Vector3Int position)
        {
            return visitedNodesDict.ContainsKey(position);
        }

        public IEnumerable<Vector3Int> GetRangePositions() => visitedNodesDict == null ? new List<Vector3Int>() : visitedNodesDict.Keys;
    }
}

