using UnityEngine;

namespace MVC.Controller.Tile
{
    [CreateAssetMenu(fileName = "Movement Cost Change", menuName = "Configs/Elements/Tile Effects/Movement Cost Change")]
    public class MovementCostChange : TileEffect
    {
        [SerializeField] private float changeMovementValueBy;

        public override int GetTileCost(int currentCost)
        {
            return Mathf.FloorToInt(changeMovementValueBy * currentCost);
        }
    }
}


