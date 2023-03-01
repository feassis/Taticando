using MVC.Controller.Unit;
using UnityEngine;
using NaughtyAttributes;
using MVC.Model.Elements;
using System;
using MVC.Model.Combat;
using System.Collections.Generic;

namespace MVC.Model.Unit
{
    [CreateAssetMenu(fileName = "UnitSetup", menuName = "Configs/Unit")]
    [Serializable]
    public class UnitSetupEntry : ScriptableObject
    {
        public TeamEnum Team;
        public ActionRangeInfo ActionInfo;
        public ElementsEnum PrimaryElement;
        [ShowAssetPreview(128, 128)] public Sprite CharacterSprite;
        public string CharacterDescription;
    }

    [CreateAssetMenu(fileName = "Unit Library", menuName = "Configs/Unit Library")]
    public class UnitLibrary : ScriptableObject
    {
        [SerializeField] private List<UnitSetupEntry> units;

        public List<UnitSetupEntry> GetUnitsDataOfTeam(TeamEnum team)
        {
            return units.FindAll(u => u.Team == team);
        }
    }
}

