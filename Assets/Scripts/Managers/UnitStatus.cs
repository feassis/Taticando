using System;

namespace MVC.Model.Unit
{
    [Serializable]
    public class UnitStatus
    {
        public ActionType Type;
        public float Stat;

        public UnitStatus(ActionType type, float stat)
        {
            Type = type;
            Stat = stat;
        }
    }
}
