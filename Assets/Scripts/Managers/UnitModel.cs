using System;
using UnityEngine;

public class UnitModel
{
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int movementPoints = 20;
    [SerializeField] private int actionPoints = 1;

    private int currentMovementpoints = 20;
    private int currentActionpoints = 1; 
    private int currentHP = 10;

    private int shield;

    private int attackPower = 1;

    private ElementsModel elements = new ElementsModel();
    private TeamEnum team;

    public int GetAttackPowuer() => attackPower;
    public TeamEnum GetTeam() => team;
    public Action<int, int> OnDamageReceived;
    public Action<int> OnShieldChanged;

    public UnitModel(TeamEnum team)
    {
        this.team = team;
    }

    public void ResetUnitTurn()
    {
        currentMovementpoints = movementPoints;
        currentActionpoints = actionPoints;
        currentHP = maxHp;
    }

    public void ApplyElement(ElementsEnum elementType, int charges)
    {
        elements.AddElement(elementType, charges);
    }

    public ElementsEnum GetElementsOnUnit()
    {
        return elements.Elements;
    }

    public bool HasActionsToDo()
    {
        return actionPoints > 0;
    }

    public void GainShield(int amount)
    {
        shield += amount;
        OnShieldChanged?.Invoke(shield);
    }

    public int GetCurrentMovementPoints()
    {
        return currentMovementpoints;
    }

    public void SpentMovementPoints(int cost)
    {
        currentMovementpoints -= cost;
    }

    public void SpentActionPoints(int cost)
    {
        actionPoints -= cost;
    }

    public int GetMaxHp() => maxHp;
    public int GetCurrentHP() => currentHP;

    public (int damageToShield, int remainingDamage) DamageShield(int dmg)
    {
        int damageToShield = Mathf.Clamp(dmg, 0, shield);

        shield -= damageToShield;

        int remainingDamage = dmg - damageToShield > 0 ? dmg - damageToShield : 0;

        OnShieldChanged?.Invoke(shield);

        return (damageToShield, remainingDamage);
    }

    public int ApplyDamage(int dmg)
    {
        currentHP -= dmg;
        Mathf.Clamp(currentHP, 0, maxHp);

        OnDamageReceived?.Invoke(currentHP, maxHp);

        return dmg;
    }
}
