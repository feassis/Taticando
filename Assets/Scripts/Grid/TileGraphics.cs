using MVC.Controller.Combat;
using MVC.Controller.Grid;
using MVC.Model.Combat;
using MVC.Model.Tile;
using MVC.View.Elements;
using MVC.View.Grid;
using MVC.View.VFX;
using Tools;
using UnityEngine;

namespace MVC.View.Tile
{
    [SelectionBase]
    public class TileGraphics : MonoBehaviour
    {
        [SerializeField] private GridCoordinates gridCoordinates;

        [SerializeField] private GlowHighlight highlight;

        [SerializeField] private TileElementGraphics tileElements;

        public TileType TileType; 

        public Vector3Int Coords => gridCoordinates.GetCoords();

        private void Awake()
        {
            if (gridCoordinates == null)
            {
                gridCoordinates = GetComponent<GridCoordinates>();
            }

            if (highlight == null)
            {
                highlight = GetComponent<GlowHighlight>();
            }
        }

        private void Start()
        {
            ServiceLocator.GetService<GridService>().OnElementApplied += UpdateElementsVisibility;
            ServiceLocator.GetService<CombatManager>().OnTeamTurnStart += UpdateTileOnTurnStart;

        }

        private void UpdateTileOnTurnStart(TeamEnum team)
        {
            UpdateElementsVisibility();
        }

        private void UpdateElementsVisibility()
        {
            tileElements.UpdateElementsVisibility(Coords);
        }

        public void EnableHighlight()
        {
            highlight.ToggleGlow(true);
        }

        public void DisableHighlight()
        {
            highlight.ToggleGlow(false);
        }

        public void ResetHightlight()
        {
            highlight.ResetGlowHighlight();
        }

        public void HighlightPath()
        {
            highlight.HighlightValidPath();
        }

        public void ResetActionHightlight()
        {
            highlight.ToggleGlow(false);//change hoghlight when range is working
        }

        public void HighlightActionRange()
        {
            highlight.ToggleGlow(true);//change hoghlight when range is working
        }

        private void OnDestroy()
        {
            ServiceLocator.GetService<GridService>().OnElementApplied -= UpdateElementsVisibility;
            ServiceLocator.GetService<CombatManager>().OnTeamTurnStart -= UpdateTileOnTurnStart;
        }
    }

}
