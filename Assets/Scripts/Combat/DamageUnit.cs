using MVC.Controler.Combat;
using MVC.View.Unit;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Unit", menuName = "Units/Action/Damage Unit")]
public class DamageUnit : UnitAction
{
    [SerializeField] private int damage;

    public override int Execute(UnitGraphics unit)
    {
        var combatManager = ServiceLocator.GetService<CombatManager>();

        return combatManager.DamageUnit(unit, damage);
    }
}
