using MVC.View.Unit;
using Tools;

namespace MVC.Controler.Combat
{
    public class UnitInCombat
    {
        public UnitGraphics UnitOnScene;
        public UnitModel UnitData;

        public int GetEstimatedDamage(int dmg)
        {
            return dmg; //change it after implementing shild system
        }

        public void GainShield(int amount)
        {
            UnitData.GainShield(amount);
        }

        public int ApplyDamage(int dmg)
        {
            return UnitData.ApplyDamage(dmg);
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
    }
}
