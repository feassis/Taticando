using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MVC.Model.Elements;
using Tools;

namespace MVC.View.UI
{
    public class PartySelectionCharacterGraphics : MonoBehaviour
    {
        [SerializeField] private Image characterImage;
        [SerializeField] private Image elementImage;
        [SerializeField] private TextMeshProUGUI elementText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private AreaOfEffectDisplayerGraphics areaOfEffectDisplay;

        private Transform canvasTransform;

        public void Setup(PlayerUnit unit, Transform canvasTransform)
        {
            characterImage.sprite = unit.CharacterSprite;
            elementText.text = unit.PrimaryElement.ToString();
            descriptionText.text = unit.CharacterDescription;
            areaOfEffectDisplay.Setup(unit.RangeInfo, canvasTransform);

            var skill = ServiceLocator.GetService<PlayerService>().GetSkillByUnitAndType(unit, 
                Controller.Combat.SkillTypeEnum.AreaOfEffect);
            skill.PartyDragable = areaOfEffectDisplay;

            this.canvasTransform = canvasTransform;
        }

        public AreaOfEffectDisplayerGraphics GetAreaOfEffectDisplay()
        {
            return areaOfEffectDisplay;
        }
    }
}

