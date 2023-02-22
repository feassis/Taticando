using MVC.Model.Combat;
using MVC.Model.Elements;
using System;
using System.Collections.Generic;

namespace MVC.Controller.Elements
{
    [Serializable]
    public class ElementOnTileEffect
    {
        public ElementsEnum Element;
        public List<TileEffectByTeam> EffectByTeam;

        public int GetTileCostAfterEffect(int defaultCost, TeamEnum team)
        {
            var desiredTileEffect = EffectByTeam.Find(t => t.Team == team);

            if (desiredTileEffect == null)
            {
                return defaultCost;
            }

            return desiredTileEffect.GetTileCostAfterEffect(defaultCost);
        }
    }
}

