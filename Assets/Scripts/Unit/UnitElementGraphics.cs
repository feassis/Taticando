using MVC.Model.Elements;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.View.Elements
{
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
}


