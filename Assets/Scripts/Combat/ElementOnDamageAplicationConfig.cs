using MVC.Controller.Combat;
using MVC.Model.Combat;
using MVC.View.Unit;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.Controller.Elements
{
    [CreateAssetMenu(fileName = "Element on Damage Application Config", menuName = "Configs/Elements/On Damage Application Config")]
    public class ElementOnDamageAplicationConfig : ScriptableObject
    {
        [SerializeField] private List<TeamOnDamageEffects> DamageEffects;

        public int GetModfiedDamage(UnitGraphics unit, int damage, DamageType type)
        {
            var team = ServiceLocator.GetService<CombatManager>().GetTeamOfAUnit(unit);
            var teamEffect = DamageEffects.Find(e => e.Team == team);

            if (teamEffect != null)
            {
                return teamEffect.GetModifiedDamage(unit, damage, type);
            }

            return damage;
        }
    }
}

