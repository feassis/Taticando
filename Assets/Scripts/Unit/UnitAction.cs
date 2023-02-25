using MVC.Model.Unit;
using MVC.View.Unit;
using UnityEngine;

namespace MVC.Controller.Unit
{
    public abstract class UnitAction : ScriptableObject
    {
        [SerializeField] protected ActionRangeInfo actionInfo;
        [SerializeField] protected ActionType actionType;
        public abstract int Execute(UnitGraphics unit);

        public ActionRangeInfo GetRangeInfo()
        {
            return actionInfo;
        }
    }
}


