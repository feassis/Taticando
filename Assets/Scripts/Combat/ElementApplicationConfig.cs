using MVC.Model.Elements;
using MVC.Model.Unit;
using MVC.View.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace MVC.Controller.Unit
{
    [CreateAssetMenu(fileName = "Element Application Config", menuName = "Configs/Elements/Application Config")]
    public class ElementApplicationConfig : ScriptableObject
    {
        [SerializeField] private List<ElementApplicationAction> actionsOnElementApplication;

        public void ActionOnApplication(UnitGraphics unit, ElementsEnum element)
        {
            ElementApplicationAction elementAction = actionsOnElementApplication.Find(e => e.Element == element);

            if (elementAction == null)
            {
                return;
            }

            elementAction.ExecuteElementApplicationActions(unit);
        }
    }
}


