using UnityEngine;

namespace MVC.Model.Elements 
{
    public class ElementsModel
    {
        public ElementsEnum Elements;
        public int Duration;

        public void AddElement(ElementsEnum element, int charges)
        {
            Elements |= element;
            Duration = Mathf.Max(charges, Duration);
        }

        public bool IsTileAfflictedByElement(ElementsEnum element)
        {
            return Elements.HasFlag(element);
        }

        public void ConsumeDuration(int amount)
        {
            Duration -= amount;

            if (Duration <= 0)
            {
                Elements = ElementsEnum.None;
            }
        }
    }
}

