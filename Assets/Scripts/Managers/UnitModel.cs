using UnityEngine;

public class UnitModel
{
    [SerializeField] private int maxHp;
    [SerializeField] private int movementPoints = 20;
    [SerializeField] private int actionPoints = 1;

    private int currentMovementpoints = 20; //when spawn is made remoce this value initialization
    private int currentActionpoints = 1; //when spawn is made remoce this value initialization

    public void ResetUnitTurn()
    {
        currentMovementpoints = movementPoints;
        currentActionpoints = actionPoints;
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
}
