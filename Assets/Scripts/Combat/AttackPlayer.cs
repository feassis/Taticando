using MVC.Controller.Combat;
using MVC.Model.Combat;
using MVC.Model.Unit;
using MVC.View.Unit;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.Controller.Unit
{
    [CreateAssetMenu(fileName = "Attack Player", menuName = "Units/Action/Enemy/Attack Player")]
    public class AttackPlayer : EnemyBasicAction
    {
        [SerializeField]
        private AttackType attackType;
        [SerializeField]
        private DamageType damageType;

        public override int Execute(UnitGraphics unit)
        {
            List<UnitInCombat> possibleTargets = GetPossibleTargets(unit);

            var combatManager = ServiceLocator.GetService<CombatManager>();

            UnitModel unitInCombat = combatManager.GetUnitOfATeam(unit).UnitData;

            float attackPower = unitInCombat.GetActionModifier(actionType);

            return Attack(Mathf.RoundToInt(attackPower), possibleTargets);
        }

        private int Attack(int dmg, List<UnitInCombat> possibleTargets)
        {
            return attackType switch
            {
                AttackType.AttackAllTargets => AttackAllTargets(dmg, possibleTargets),
                AttackType.AttackTargetWithLowerHP => AttackTargetWithLowerHP(dmg, possibleTargets),
                AttackType.AttackTargetToCauseMostDamage => AttackTargetThatSufferMoreDamage(dmg, possibleTargets),
                _ => throw new System.NotImplementedException()
            };
        }

        private int AttackTargetThatSufferMoreDamage(int dmg, List<UnitInCombat> possibleTargets)
        {
            UnitInCombat targetUnit = null;

            foreach (var target in possibleTargets)
            {
                if (target.UnitData.GetTeam() != TeamEnum.Player)
                {
                    continue;
                }

                if (targetUnit == null)
                {
                    targetUnit = target;
                    continue;
                }

                if (targetUnit.GetEstimatedDamage(dmg) < targetUnit.GetEstimatedDamage(dmg))
                {
                    targetUnit = target;
                }
            }

            if (targetUnit != null)
            {
                return targetUnit.ApplyDamage(dmg, damageType);
            }

            return 0;
        }

        private int AttackTargetWithLowerHP(int dmg, List<UnitInCombat> possibleTargets)
        {
            UnitInCombat targetUnit = null;

            foreach (var target in possibleTargets)
            {
                if (target.UnitData.GetTeam() != TeamEnum.Player)
                {
                    continue;
                }

                if (targetUnit == null)
                {
                    targetUnit = target;
                    continue;
                }

                if (targetUnit.UnitData.GetCurrentHP() > targetUnit.UnitData.GetCurrentHP())
                {
                    targetUnit = target;
                }
            }

            if (targetUnit != null)
            {
                return targetUnit.ApplyDamage(dmg, damageType);
            }

            return 0;
        }

        private int AttackAllTargets(int dmg, List<UnitInCombat> possibleTargets)
        {
            int totalDamageDelt = 0;

            foreach (var target in possibleTargets)
            {
                if (target.UnitData.GetTeam() == TeamEnum.Player)
                {
                    totalDamageDelt += target.ApplyDamage(dmg, damageType);
                }
            }

            return totalDamageDelt;
        }

        private enum AttackType
        {
            AttackAllTargets = 0,
            AttackTargetWithLowerHP = 1,
            AttackTargetToCauseMostDamage = 2,
        }
    }

}

