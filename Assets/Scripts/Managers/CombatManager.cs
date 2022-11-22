using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager
{
    private int currentTeamTurnIndex;

    public Action OnTurnChange;

    private List<Team> InCombatTeams = new List<Team>();

    public Team GetCurrentTeamTurn() => InCombatTeams[currentTeamTurnIndex % InCombatTeams.Count];

    public void CheckTeamHasActionsToDo()
    {
        if (!GetCurrentTeamTurn().HasActionsToDo())
        {
            ChangeTurn();
        }
    }

    public void ChangeTurn()
    {
        currentTeamTurnIndex++;
        OnTurnChange?.Invoke();
    }
}

public class Team
{
    public TeamEnum MyTeam;
    private List<UnitInCombat> unitsInCombat;

    public bool HasActionsToDo()
    {
        foreach (var unit in unitsInCombat)
        {
            if (unit.UnitData.HasActionsToDo())
            {
                return true;
            }
        }

        return false;
    }
}

public class UnitInCombat
{
    public GameObject UnityOnScene; //this will probably change to a ID system when unit spawn is decided
    public UnitModel UnitData;
}

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
}
