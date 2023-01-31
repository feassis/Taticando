using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Element Tile Effects Config", menuName = "Configs/Elements/Effects On Tile Config")]
public class ElementTileEffectConfig : ScriptableObject
{
    [SerializeField] List<ElementOnTileEffect> ElementsTileEffects;

    public int GetTileMovementCostAffectedByElement(int defaultCost, ElementsEnum element, TeamEnum team)
    {
        int currentCost = defaultCost;
        foreach (var elementOption in (ElementsEnum[])Enum.GetValues(typeof(ElementsEnum)))
        {
            var elementEffect = ElementsTileEffects.Find(e => e.Element == elementOption);

            if(elementEffect == null)
            {
                continue;
            }

            if (element.HasFlag(elementEffect.Element))
            {
                currentCost = elementEffect.GetTileCostAfterEffect(currentCost, team);
            }
        }

        return currentCost;
    }
}
