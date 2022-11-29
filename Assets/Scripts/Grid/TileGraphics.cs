using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

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

    private void OnDestroy()
    {
        ServiceLocator.GetService<GridService>().OnElementApplied -= UpdateElementsVisibility;
    }
}
