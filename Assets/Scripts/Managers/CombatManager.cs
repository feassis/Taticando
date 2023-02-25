using MVC.Controller.Unit;
using MVC.Model.Combat;
using MVC.Model.Elements;
using MVC.Model.Unit;
using MVC.View.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

namespace MVC.Controller.Combat
{
    public class CombatManager
    {
        private int currentTeamTurnIndex;

        public Action OnTurnChange;

        private List<Team> InCombatTeams = new List<Team>();

        public Team GetCurrentTeamTurn() => currentTeamTurnIndex == 0 ? InCombatTeams[0] : InCombatTeams[currentTeamTurnIndex % InCombatTeams.Count];

        public Action<TeamEnum> OnTeamTurnStart;

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
            OnTeamTurnStart?.Invoke(GetCurrentTeamTurn().MyTeam);
        }

        public void AddUnitToTeam(TeamEnum desiredTeam, UnitGraphics unitGraphicsObject)
        {
            Team selectedTeam = InCombatTeams.Find(t => t.MyTeam == desiredTeam);

            if (selectedTeam == null)
            {
                selectedTeam = new Team();
                selectedTeam.MyTeam = desiredTeam;
                InCombatTeams.Add(selectedTeam);
            }

            selectedTeam.UnitsInCombat.Add(new UnitInCombat(unitGraphicsObject, desiredTeam));
        }

        public void SpendMovementPointsOfAUnit(UnitGraphics unitObject, int cost)
        {
            var unitModel = GetUnitOfATeam(unitObject);
            unitModel.UnitData.SpentMovementPoints(cost);
        }

        public void ExecuteActionOfSelectedUnit()
        {
            var unitManager = ServiceLocator.GetService<UnitManager>();
            var selectedUnit = unitManager.GetSelectedUnitReference();
            unitManager.UseSelectedUnityAction();
            var unitModel = GetUnitOfATeam(selectedUnit);
            unitModel.UnitData.SpentActionPoints(1);//this number will change after implementation o tool to create characters
            CheckTeamHasActionsToDo();
        }

        public bool IsThisUnitTurn(UnitGraphics unit)
        {
            var team = GetCurrentTeamTurn();

            return team.IsUnitOnTeam(unit);
        }

        public int GetUnitMovementPoints(UnitGraphics desiredUnit)
        {
            UnitModel unit = GetUnitOfATeam(desiredUnit).UnitData;

            if (unit == null)
            {
                Debug.LogError("unit not on a team");
                return 0;
            }

            return unit.GetCurrentMovementPoints();
        }

        public TeamEnum GetTeamOfAUnit(UnitGraphics desiredUnit)
        {
            foreach (var team in InCombatTeams)
            {
                foreach (var unit in team.UnitsInCombat)
                {
                    if(unit.UnitOnScene == desiredUnit)
                    {
                        return team.MyTeam;
                    }
                }
            }

            return TeamEnum.None;
        }

        public void ApplyElementOnUnits(List<Vector3Int> unitsAffectedPosition, ElementsEnum element)
        {
            var unitsOnRange = GetUnitsOnRange(unitsAffectedPosition);

            foreach (UnitInCombat unit in unitsOnRange)
            {
                unit.ApplyElement(element);
            }
        }

        public List<UnitInCombat> GetUnitsOnRange(List<Vector3Int> unitsAffectedPosition)
        {
            var unitsOnField = GetUnitsOnField();
            List<UnitInCombat> unitsOnRange = new List<UnitInCombat>();

            foreach (UnitInCombat unit in unitsOnField)
            {
                Vector3Int unitCoord = unit.UnitOnScene.GetMyTilePosition();
                if (unitsAffectedPosition.Contains(unitCoord))
                {
                    unitsOnRange.Add(unit);
                }
            }

            return unitsOnRange;
        }

        public void SubscribeActionToUnitOnDamage(UnitGraphics desiredUnit, Action<int, int> onDamage)
        {
            var unit = GetUnitInCombat(desiredUnit);

            unit.UnitData.OnDamageReceived += onDamage;
        }

        public void SubscribeActionToUnitOnHeal(UnitGraphics desiredUnit, Action<int, int> onHeal)
        {
            var unit = GetUnitInCombat(desiredUnit);

            unit.UnitData.OnHealReceived+= onHeal;
        }

        public void SubscribeActionToUnitShieldChange(UnitGraphics desiredUnit, Action<int> onShieldChange)
        {
            var unit = GetUnitInCombat(desiredUnit);

            unit.UnitData.OnShieldChanged += onShieldChange;
        }

        public void GiveShield(UnitGraphics desiredUnit, int amount)
        {
            var unit = GetUnitInCombat(desiredUnit);

            unit.GainShield(amount);
        }

        public int DamageUnit(UnitGraphics desiredUnit, int dmg, DamageType dmgType)
        {
            var unit = GetUnitInCombat(desiredUnit);

            return unit.ApplyDamage(dmg, dmgType);
        }

        public int HealUnit(UnitGraphics desiredUnit, int amount)
        {
            var unit = GetUnitInCombat(desiredUnit);

            return unit.Heal(amount);
        }

        public (int currentHp, int maxHP) GetUnitHPStatus(UnitGraphics desiredUnit)
        {
            var unit = GetUnitInCombat(desiredUnit);

            return (unit.UnitData.GetCurrentHP(), unit.UnitData.GetMaxHp());
        }

        public void DamageRandomUnitOfCurrentTeam()
        {
            var team = GetCurrentTeamTurn();

            int randomIndex = UnityEngine.Random.Range(0, team.UnitsInCombat.Count);

            team.UnitsInCombat[randomIndex].UnitData.ApplyDamage(1);
        }

        private UnitInCombat GetUnitInCombat(UnitGraphics desiredUnit)
        {
            foreach (var team in InCombatTeams)
            {
                foreach (UnitInCombat unit in team.UnitsInCombat)
                {
                    if (unit.UnitOnScene == desiredUnit)
                    {
                        return unit;
                    }
                }
            }

            return null;
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

        private List<UnitInCombat> GetUnitsOnField()
        {
            List<UnitInCombat> units = new List<UnitInCombat>();
            foreach (var team in InCombatTeams)
            {
                units.AddRange(team.UnitsInCombat);
            }

            return units;
        }

        public UnitInCombat GetUnitOfATeam(UnitGraphics desiredUnit)
        {
            foreach (var team in InCombatTeams)
            {
                foreach (UnitInCombat unit in team.UnitsInCombat)
                {
                    if (unit.UnitOnScene.Equals(desiredUnit))
                    {
                        return unit;
                    }
                }
            }

            return null;
        }
    }
}
