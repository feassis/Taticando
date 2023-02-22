using MVC.Controller.Elements;
using MVC.Model.Combat;
using MVC.Model.Elements;
using MVC.Model.Unit;
using MVC.View.Unit;
using Tools;

namespace MVC.Controller.Combat
{
    public class UnitInCombat
    {
        public UnitGraphics UnitOnScene;
        public UnitModel UnitData;

        public int GetUnitShiled()
        {
            return UnitData.GetCurrentShield();
        }

        public float GetActionModifier(ActionType type)
        {
            return UnitData.GetActionModifier(type);
        }

        public int GetEstimatedDamage(int dmg)
        {
            return dmg; //change it after implementing shild system
        }

        public void GainShield(int amount)
        {
            UnitData.GainShield(amount);
        }

        public int ApplyDamage(int dmg, DamageType type)
        {
            var elementService = ServiceLocator.GetService<ElementService>();

            var modifiedDamage = elementService.GetDamageModifiedByElement(UnitOnScene, dmg, type);

            return UnitData.ApplyDamage(modifiedDamage);
        }

        public int Heal(int amount)
        {
            return UnitData.Heal(amount);
        }

        public UnitInCombat(UnitGraphics unitGraphicsObject, TeamEnum team)
        {
            UnitOnScene = unitGraphicsObject;
            UnitData = new UnitModel(team);
        }

        public void ApplyElement(ElementsEnum element)
        {
            UnitData.ApplyElement(element, 1);

            ServiceLocator.GetService<ElementService>().CallElementApplication(UnitOnScene, element);

            UnitOnScene.UpdateElementVisibility(UnitData.GetElementsOnUnit());
        }

        public ElementsEnum GetElementsAfflictingUnit() => UnitData.GetElementsOnUnit();
    }
}
