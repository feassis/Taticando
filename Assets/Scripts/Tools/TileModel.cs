using MVC.Controller.Combat;
using MVC.Controller.Elements;
using MVC.Model.Combat;
using MVC.Model.Elements;
using System;
using Tools;
using UnityEngine;

namespace MVC.Model.Tile
{
    public class TileModel
    {
        private ElementsModel elements;
        private TileType type;

        public TileModel(TileType type)
        {
            this.type = type;
            elements = new ElementsModel();
        }

        public int GetDefaultCost(TileType tileType) => tileType switch
        {
            TileType.Default => 10,
            TileType.Difficult => 20,
            TileType.Road => 5,
            _ => throw new Exception($"Tile Type {tileType} Not Supported"),
        };

        public int GetCost(TeamEnum team)
        {
            int currentCost = GetDefaultCost(type);

            currentCost = ServiceLocator.GetService<ElementService>().GetTileMovementCostAfterElement(currentCost, elements.Elements, team);
            return currentCost;
        }

        public bool IsTileAfflictedByElement(ElementsEnum element)
        {
            return elements.IsTileAfflictedByElement(element);
        }

        public void ApplyElement(ElementsEnum elementsEnum)
        {
            elements.AddElement(elementsEnum, 1);
            ServiceLocator.GetService<CombatManager>().OnTeamTurnStart += StartOfTurnTick;
            Debug.Log($"Element {elements.Elements}, shold have {elementsEnum}");
        }

        public bool IsObstacle()
        {
            return type == TileType.Obstacle || type == TileType.Water;
        }

        public void StartOfTurnTick(TeamEnum team)
        {
            if (team == TeamEnum.Player)
            {
                elements.ConsumeDuration(1);
            }

            if (elements.Elements == ElementsEnum.None)
            {
                ServiceLocator.GetService<CombatManager>().OnTeamTurnStart -= StartOfTurnTick;
            }
        }
    }
}
