using MVC.View.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Tools;

namespace MVC.View.UI
{
    public class AreaOfEffectDisplayerGraphics : DragableSkillPartySceneGraphics, IDropHandler
    {
        [SerializeField] private Image areaOfEffectImage;
        [SerializeField] private GameObject centralPlasyerIndication;
        [SerializeField] private GameObject insidePlasyerIndication;
        [SerializeField] private GameObject outerPlasyerIndication;
        [SerializeField] private TextMeshProUGUI offsetText;
        [SerializeField] private TextMeshProUGUI radiousText;

        [SerializeField] private List<AreaOfEffectPrefab> areaOfEffectSetup;

        [Serializable]
        private struct AreaOfEffectPrefab
        {
            public NeighbourhoodType NeighbourhoodType;
            public Sprite AreaOfEffectIcon;
        }

        public void Setup(Controller.Unit.ActionRangeInfo rangeInfo, Transform canvasTransform)
        {
            SetupPlayerIndication(rangeInfo.ActionDistance, rangeInfo.ActionRangeAmount);

            SetupAreaOfEffect(rangeInfo);

            this.canvasTransform = canvasTransform;
        }

        public override void UpdateGraphics()
        {
            base.UpdateGraphics();

            var playerService = ServiceLocator.GetService<PlayerService>();

            var skill = (AreaOfEffectSkill) playerService.GetSkillByItsDragable(this);

            SetupAreaOfEffect(skill.RangeInfo);
        }

        private void SetupAreaOfEffect(Controller.Unit.ActionRangeInfo rangeInfo)
        {
            var desiredOption = areaOfEffectSetup.Find(a => a.NeighbourhoodType == rangeInfo.NeighbourhoodType);
            areaOfEffectImage.sprite = desiredOption.AreaOfEffectIcon;
            offsetText.text = rangeInfo.ActionDistance.ToString();
            radiousText.text = rangeInfo.ActionRangeAmount.ToString();
        }

        private void SetupPlayerIndication(int offset, int range)
        {
            centralPlasyerIndication.SetActive(false);
            insidePlasyerIndication.SetActive(false);
            outerPlasyerIndication.SetActive(false);

            if(offset == 0)
            {
                centralPlasyerIndication.SetActive(true);

                return;
            }

            if(offset < range)
            {
                insidePlasyerIndication.SetActive(true);
                return;
            }

            outerPlasyerIndication.SetActive(true);
        }

        public void OnDrop(PointerEventData eventData)
        {
            var playerService = ServiceLocator.GetService<PlayerService>();
            var partySceneGraphics = ServiceLocator.GetService<PartySetupSceneGraphics>();

            var skillBeingDraged = partySceneGraphics.GetSkillBeingDraged();

            playerService.SwapDragableSkill(skillBeingDraged, this);
            ServiceLocator.GetService<PartySetupSceneGraphics>().DeregisterSkillBeingDraged();
        }
    }
}

