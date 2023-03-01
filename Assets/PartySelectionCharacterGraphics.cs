using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MVC.Model.Elements;

namespace MVC.View.UI
{
    public class PartySelectionCharacterGraphics : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private Image elementImage;
        [SerializeField] private TextMeshProUGUI elementText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private AreaOfEffectDisplayerGraphics areaOfEffectDisplay;

        public void Setup(Sprite characterSprite, ElementsEnum element, 
            string description, Controller.Unit.ActionRangeInfo rangeInfo)
        {
            characterImage.sprite = characterSprite;
            elementText.text = element.ToString();
            descriptionText.text = description;
            areaOfEffectDisplay.Setup(rangeInfo);
        }
    }
}

