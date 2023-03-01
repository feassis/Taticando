using MVC.View.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View.UI
{
    public class AreaOfEffectDisplayerGraphics : MonoBehaviour
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

        public void Setup(Controller.Unit.ActionRangeInfo rangeInfo)
        {
            SetupPlayerIndication(rangeInfo.ActionDistance, rangeInfo.ActionRangeAmount);

            SetupAreaOfEffect(rangeInfo);
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
    }
}

