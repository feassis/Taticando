using MVC.Controller.Elements;
using MVC.Model.Combat;
using MVC.View.Unit;
using System;
using System.Collections.Generic;

namespace MVC.Controller.Combat
{
    [Serializable]
    public class TeamOnDamageEffects
    {
        public TeamEnum Team;
        public List<ElementOnDamageEffects> DamageEffects = new List<ElementOnDamageEffects>();

        public int GetModifiedDamage(UnitGraphics unit, int damage, DamageType type)
        {
            int currentDamage = damage;

            foreach (var effect in DamageEffects)
            {
                currentDamage = effect.GetModifiedDamage(unit, currentDamage, type);
            }

            return currentDamage;
        }
    }
}

