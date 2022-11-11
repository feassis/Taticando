using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraphics : MonoBehaviour
{
    [SerializeField] private GridCoordinates gridCoordinates;


    [SerializeField] private GlowHighlight highlight;

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

    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }
}
