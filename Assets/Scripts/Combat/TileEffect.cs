using UnityEngine;

namespace MVC.Controller.Tile
{
    public abstract class TileEffect : ScriptableObject
    {
        public abstract int GetTileCost(int currentCost);
    }
}

