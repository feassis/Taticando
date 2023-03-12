using MVC.View.Grid;
using System;

namespace MVC.Controller.Unit
{
    [Serializable]
    public class ActionRangeInfo
    {
        public int ActionDistance;
        public NeighbourhoodType NeighbourhoodType;
        public int ActionRangeAmount;

        public ActionRangeInfo()
        {

        }

        public ActionRangeInfo(int actionDistance, NeighbourhoodType neighbourhoodType, int actionRangeAmount)
        {
            ActionDistance = actionDistance;
            NeighbourhoodType = neighbourhoodType;
            ActionRangeAmount = actionRangeAmount;
        }
    }
}


