using UnityEngine;

public class UnitModel
{
    [SerializeField] private int maxHp;
    [SerializeField] private int movementPoints = 20;

    private int currentMovementpoints = 20; //when spawn is made remoce this value initialization

    public void ResetUnitTurn()
    {
        currentMovementpoints = movementPoints;
    }

    public bool HasActionsToDo()
    {
        return currentMovementpoints > 0;
    }

    public int GetCurrentMovementPoints()
    {
        return currentMovementpoints;
    }

    public void SpentMovementPoints(int cost)
    {
        currentMovementpoints -= cost;
    }
}
