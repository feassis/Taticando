using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class TileElementGraphics : MonoBehaviour
{
    [SerializeField] private Transform elementsHolder;
    [SerializeField] private List<ElementGraphicIsntance> elements;

    public void UpdateElementsVisibility(Vector3Int position)
    {
        var gridService = ServiceLocator.GetService<GridService>();

        foreach (var element in elements)
        {
            element.elementGraphics.SetActive(gridService.IsTileAfflictedByElement(position, element.element));
        }
    }
}

[Serializable]
public struct ElementGraphicIsntance
{
    public ElementsEnum element;

    public GameObject elementGraphics;
}
