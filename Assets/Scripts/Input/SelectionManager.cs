using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    public Action<GameObject> OnUnitSeleced;
    public Action<GameObject> OnTerrainSelected;

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

        var unitManager = ServiceLocator.GetService<UnitManager>();

        OnUnitSeleced += unitManager.HandleUnitSelected;
        OnTerrainSelected += unitManager.HandleTerrainSelected;
    }

    private void OnDestroy()
    {
        playerInput.DeregisterToPointerClicked(HandleClick);
        ServiceLocator.DeregisterService<SelectionManager>();
    }

    public void HandleClick(Vector3 mousePosition)
    {
        var grid = ServiceLocator.GetService<IGrid>();

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (FindTarget(mousePosition, out GameObject result))
        {
            if (UnitSelected(result))
            {
                OnUnitSeleced?.Invoke(result);
            }
            else if(TileSelected(result))
            {
                OnTerrainSelected?.Invoke(result);
            }
        }
    }



    private bool UnitSelected(GameObject result) => result.GetComponent<UnitGraphics>() != null;
    private bool TileSelected(GameObject result) => result.GetComponent<TileGraphics>() != null;

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, selectionMask))
        {
            var hitedObject = hit.collider.gameObject;

            var unitObject = hitedObject.GetComponentInParent<UnitGraphics>();

            if (unitObject == null)
            {
                result = hitedObject.GetComponentInParent<TileGraphics>().gameObject;
            }
            else
            {
                result = unitObject.gameObject;
            }

            return true;
        }

        result = null;
        return false;
    }
}
