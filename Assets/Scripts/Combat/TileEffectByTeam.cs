using System;
using System.Collections.Generic;

[Serializable]
public class TileEffectByTeam
{
    public TeamEnum Team;

    public List<TileEffect> TileEffects;

    public int GetTileCostAfterEffect(int defaultCost)
    {
        int currentCost = defaultCost;
        foreach (var effect in TileEffects)
        {
            currentCost = effect.GetTileCost(currentCost);
        }

        return currentCost;
    }
}
