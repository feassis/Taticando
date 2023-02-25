using MVC.Model.Combat;
using MVC.View.Unit;
using System;
using UnityEngine;

namespace MVC.Controller.Elements
{
    [Serializable]
    [CreateAssetMenu(fileName = "Element On Damage Effect", menuName = "Configs/Elements/On Damage Effect")]
    public abstract class ElementOnDamageEffects : ScriptableObject
    {
        public virtual int GetModifiedDamage(UnitGraphics unit, int damage, DamageType type)
        {
            return damage;
        }
    }
}

