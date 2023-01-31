using MVC.View.Unit;
using Tools;
using UnityEngine;

public class ElementService : MonoBehaviour
{
    [SerializeField] private ElementApplicationConfig elementConfig;
    [SerializeField] private ElementTileEffectConfig elementTileEffect;

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

    private void OnDestroy()
    {
        ServiceLocator.DeregisterService<ElementService>();
    }
}
