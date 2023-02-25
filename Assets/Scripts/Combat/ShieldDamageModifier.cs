using MVC.Controller.Combat;
using MVC.Model.Combat;
using MVC.View.Unit;
using System;
using Tools;
using UnityEngine;

namespace MVC.Controller.Elements
{
    [Serializable]
    [CreateAssetMenu(fileName = "Element On Damage Effect With Element To Ignore", menuName = "Configs/Elements/On Damage Effect Ignoring Elements")]
    public class ShieldDamageModifier : ElementOnDamageEffects
    {
        [SerializeField] private DamageType damageType;
        [SerializeField] protected float damageModifierWhenShielded;
        [SerializeField] protected float damageModifierWhenNotShielded;

        public override int GetModifiedDamage(UnitGraphics unit, int damage, DamageType type)
        {
            var unitInCombat = ServiceLocator.GetService<CombatManager>().GetUnitOfATeam(unit);

            if (damageType != type)
            {
                return damage;
            }


            if (unitInCombat.GetUnitShiled() > 0)
            {
                return Mathf.RoundToInt(damageModifierWhenShielded * damage);
            }

            return Mathf.RoundToInt(damageModifierWhenNotShielded * damage);
        }
    }
}


