using MVC.View.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.Controller.Grid
{
    public static class Direction
    {
        public static List<Vector3Int> DirectionCrossNeighbourhood = new List<Vector3Int>
    {
        new Vector3Int(1,0,0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

        public static List<Vector3Int> LineUp = new List<Vector3Int>
    {
        new Vector3Int(0, 0, 1)
    };

        public static List<Vector3Int> LineDown = new List<Vector3Int>
    {
        new Vector3Int(0, 0, -1)
    };

        public static List<Vector3Int> LineRight = new List<Vector3Int>
    {
        new Vector3Int(1, 0, 0)
    };

        public static List<Vector3Int> LineLeft = new List<Vector3Int>
    {
        new Vector3Int(-1, 0, 0)
    };

        public static List<Vector3Int> CenteredSquare = new List<Vector3Int>
    {
        new Vector3Int(1,0,0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
        new Vector3Int(1,0,1),
        new Vector3Int(-1, 0, 1),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(1, 0, -1),
    };

        public static List<Vector3Int> UpCone = new List<Vector3Int>
    {
        new Vector3Int(1,0,1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(-1, 0, 1),
    };

        public static List<Vector3Int> GetDirectionList(NeighbourhoodType type) => type switch
        {
            NeighbourhoodType.Cross => DirectionCrossNeighbourhood,
            NeighbourhoodType.LineUp => LineUp,
            NeighbourhoodType.LineDown => LineDown,
            NeighbourhoodType.LineRight => LineRight,
            NeighbourhoodType.LineLeft => LineLeft,
            NeighbourhoodType.CenteredSquare => CenteredSquare,
            NeighbourhoodType.UpCone => UpCone,
            _ => throw new System.NotImplementedException(),
        };
    }
}

