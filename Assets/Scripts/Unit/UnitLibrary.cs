using UnityEngine;
using MVC.Model.Combat;
using System.Collections.Generic;

namespace MVC.Model.Unit
{
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

