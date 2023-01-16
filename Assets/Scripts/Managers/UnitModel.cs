using System;
using UnityEngine;

public class UnitModel
{
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int movementPoints = 20;
    [SerializeField] private int actionPoints = 1;

    private int currentMovementpoints = 20; //when spawn is made remoce this value initialization
    private int currentActionpoints = 1; //when spawn is made remoce this value initialization
    private int currentHP = 10;

    private ElementsModel elements = new ElementsModel();
    private TeamEnum team;

    public Action<int, int> OnDamageReceived;

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

    public void ApplyDamage(int dmg)
    {
        currentHP -= dmg;
        Mathf.Clamp(currentHP, 0, maxHp);

        OnDamageReceived?.Invoke(currentHP, maxHp);
    }
}
