using MVC.Controller.Unit;
using MVC.Model.Combat;
using MVC.Model.Elements;
using MVC.View.Unit;
using Tools;
using UnityEngine;

namespace MVC.Controller.Elements
{
    public class ElementService : MonoBehaviour
    {
        [SerializeField] private ElementApplicationConfig elementConfig;
        [SerializeField] private ElementTileEffectConfig elementTileEffect;
        [SerializeField] private ElementOnDamageAplicationConfig elementOnDamageEffect;

        private void Awake()
        {
            ServiceLocator.RegisterService<ElementService>(this);
        }

        public void CallElementApplication(UnitGraphics unit, ElementsEnum element)
        {
            elementConfig.ActionOnApplication(unit, element);
        }

        public int GetTileMovementCostAfterElement(int defaultCost, ElementsEnum element, TeamEnum team)
        {
            return elementTileEffect.GetTileMovementCostAffectedByElement(defaultCost, element, team);
        }

        public int GetDamageModifiedByElement(UnitGraphics unit, int damage, DamageType type)
        {
            return elementOnDamageEffect.GetModfiedDamage(unit, damage, type);
        }

        private void OnDestroy()
        {
            ServiceLocator.DeregisterService<ElementService>();
        }
    }

}
