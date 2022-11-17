using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    private List<Vector3Int> neighbours = new List<Vector3Int>();

    [SerializeField]
    private PlayerInput playerInput;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        ServiceLocator.RegisterService<SelectionManager>(this);
        playerInput.RegisterToPointerClicked(HandleClick);
    }

    private void OnDestroy()
    {
        playerInput.DeregisterToPointerClicked(HandleClick);
        ServiceLocator.DeregisterService<SelectionManager>();
    }

    public void HandleClick(Vector3 mousePosition)
    {
        var grid = ServiceLocator.GetService<SquareGrid>();

        if (FindTarget(mousePosition, out GameObject result))
        {
            TileGraphics selectedTile = result.GetComponent<TileGraphics>();

            selectedTile.DisableHighlight();

            foreach (var neighbour in neighbours)
            {
                grid.GetTileAt(neighbour).DisableHighlight();
            }

            var graphSearch = ServiceLocator.GetService<GraphSearch>();
            BFSResult bfsResult = graphSearch.BFSGetRange(grid, selectedTile.Coords, 20);
            neighbours = new List<Vector3Int>(bfsResult.GetRangePositions());

            foreach (var neighboursPos in neighbours)
            {
                grid.GetTileAt(neighboursPos).EnableHighlight();
            }
        }
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, selectionMask))
        {
            result = hit.collider.gameObject.GetComponentInParent<TileGraphics>().gameObject;
            return true;
        }

        result = null;
        return false;
    }
}
