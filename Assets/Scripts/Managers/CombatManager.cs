using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MVC.Controler.Combat
{
    public class CombatManager
    {
        private int currentTeamTurnIndex;

        public Action OnTurnChange;

        private List<Team> InCombatTeams = new List<Team>();

        public Team GetCurrentTeamTurn() => currentTeamTurnIndex == 0 ? InCombatTeams[0] : InCombatTeams[currentTeamTurnIndex % InCombatTeams.Count];

        public void CheckTeamHasActionsToDo()
        {
            if (!GetCurrentTeamTurn().HasActionsToDo())
            {
                ChangeTurn();
            }
        }

        public void ChangeTurn()
        {
            currentTeamTurnIndex++;
            ResetTeamUnitsTurn();
            OnTurnChange?.Invoke();
        }

        public void AddUnitToTeam(TeamEnum desiredTeam, GameObject unitGraphicsObject)
        {
            Team selectedTeam = InCombatTeams.Find(t => t.MyTeam == desiredTeam);

            if (selectedTeam == null)
            {
                selectedTeam = new Team();
                selectedTeam.MyTeam = desiredTeam;
                InCombatTeams.Add(selectedTeam);
            }

            selectedTeam.UnitsInCombat.Add(new UnitInCombat(unitGraphicsObject));
        }

        public void SpendMovementPointsOfAUnit(GameObject unitObject, int cost)
        {
            var unitModel = GetUnitOfATeam(unitObject);
            unitModel.SpentMovementPoints(cost);
        }

        public bool IsThisUnitTurn(GameObject unit)
        {
            var team = GetCurrentTeamTurn();

            return team.IsUnitOnTeam(unit);
        }

        public int GetUnitMovementPoints(GameObject desiredUnit)
        {
            UnitModel unit = GetUnitOfATeam(desiredUnit);

            if (unit == null)
            {
                Debug.LogError("unit not on a team");
                return 0;
            }

            return unit.GetCurrentMovementPoints();
        }

        private List<UnitInCombat> GetUnitsOfATeam(TeamEnum teamEnum)
        {
            var units = new List<UnitInCombat>();

            foreach (var team in InCombatTeams)
            {
                if (team.MyTeam != teamEnum)
                {
                    continue;
                }

                return team.UnitsInCombat;
            }

            return null;
        }

        private void ResetTeamUnitsTurn()
        {
            var units = GetUnitsOfATeam(GetCurrentTeamTurn().MyTeam);

            foreach (var unit in units)
            {
                unit.UnitData.ResetUnitTurn();
            }
        }

        private UnitModel GetUnitOfATeam(GameObject desiredUnit)
        {
            foreach (var team in InCombatTeams)
            {
                foreach (var unit in team.UnitsInCombat)
                {
                    if (unit.UnitOnScene.Equals(desiredUnit))
                    {
                        return unit.UnitData;
                    }
                }
            }

            return null;
        }
    }

    public class Team
    {
        public TeamEnum MyTeam;
        public List<UnitInCombat> UnitsInCombat = new List<UnitInCombat>();

        public bool HasActionsToDo()
        {
            foreach (var unit in UnitsInCombat)
            {
                if (unit.UnitData.HasActionsToDo())
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsUnitOnTeam(GameObject unit)
        {
            foreach (var unitInCombat in UnitsInCombat)
            {
                if (unitInCombat.UnitOnScene.Equals(unit))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class UnitInCombat
    {
        public GameObject UnitOnScene; //this will probably change to a ID system when unit spawn is decided
        public UnitModel UnitData;

        public UnitInCombat(GameObject unitGraphicsObject)
        {
            UnitOnScene = unitGraphicsObject;
            UnitData = new UnitModel();
        }
    }
}
