using UnityEngine;

public abstract class TileEffect : ScriptableObject
{
    public abstract int GetTileCost(int currentCost);
} 
