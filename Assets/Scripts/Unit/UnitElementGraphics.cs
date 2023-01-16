using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitElementGraphics : MonoBehaviour
{
    [SerializeField] private Transform elementsHolder;
    [SerializeField] private List<ElementGraphicIsntance> elements;

    public void UpdateElementsVisibility(ElementsEnum elementsActive)
    {
        foreach (var element in elements)
        {
            element.elementGraphics.SetActive(elementsActive.HasFlag(element.element));
        }
    }

}
