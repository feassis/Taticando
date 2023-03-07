using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using NaughtyAttributes;

public class ElementIconGraphics : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text elementName;
    [SerializeField] private List<ElementIconSprite> elementIcons;

    [Serializable]
    private struct ElementIconSprite
    {
        public MVC.Model.Elements.ElementsEnum Element;
        [ShowAssetPreview(64, 64)]public Sprite Elementicon;
    }

    public void Setup(MVC.Model.Elements.ElementsEnum element)
    {
        var icon = elementIcons.Find(e => e.Element == element);
        iconImage.sprite = icon.Elementicon;
        elementName.text = element.ToString();
    }
}
