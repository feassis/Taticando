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
        [SerializeField] private ElementIconGraphics elementIconGraphics;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private AreaOfEffectDisplayerGraphics areaOfEffectDisplay;

        private Transform canvasTransform;

        public void Setup(PlayerUnit unit, Transform canvasTransform)
        {
            characterImage.sprite = unit.CharacterSprite;
            elementIconGraphics.Setup(unit.PrimaryElement);
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

        public void HighlightReleventDragableAreas(Controller.Combat.SkillTypeEnum type, bool isOn)
        {
            switch (type)
            {
                case Controller.Combat.SkillTypeEnum.AreaOfEffect:
                    areaOfEffectDisplay.ToggleHighlight(isOn);
                    break;
                case Controller.Combat.SkillTypeEnum.Element:
                    break;
            }
        }
    }
}