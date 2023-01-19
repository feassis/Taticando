using MVC.View.Unit;
using System;
using UnityEngine;


public abstract class UnitAction : ScriptableObject
{
    [SerializeField] private ActionRangeInfo actionInfo;
    public abstract int Execute(UnitGraphics unit);

    public ActionRangeInfo GetRangeInfo()
    {
        return actionInfo;
    }
}

[Serializable]
public struct ActionRangeInfo
{
    public int ActionDistance;
    public NeighbourhoodType NeighbourhoodType;
    public int ActionRangeAmount;
}

