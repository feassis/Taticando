using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCoordinates : MonoBehaviour
{
    public static float XOffset = 1f;
    public static float YOffset = 1f;
    public static float ZOffset = 1f;

    internal Vector3Int GetCoords() => offsetCoordinates;

    [Header("Offset Coordinates")]
    [SerializeField]
    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConversPositionToOffset(transform.position);
        gameObject.name = $"Tile - (x: {offsetCoordinates.x},y: {offsetCoordinates.y},z: {offsetCoordinates.z})";
    }

    private Vector3Int ConversPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / XOffset);
        int y = Mathf.RoundToInt(position.y / YOffset);
        int z = Mathf.RoundToInt(position.z / ZOffset);

        return new Vector3Int(x, y, z);
    }
}
