using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Action<Vector3> PointerClicked;

    void Update()
    {
        DetectPlayerClick();
        DetectSpaceBarDown();
    }

    private void DetectPlayerClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            PointerClicked?.Invoke(mousePos);
        }
    }

    private void DetectSpaceBarDown()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Down");
            ServiceLocator.GetService<GridService>().ApplyElementToTiles(new List<Vector3Int> { new Vector3Int(0, -1, 0) }, ElementsEnum.Geo);
            ServiceLocator.GetService<GridService>().ApplyElementToTiles(new List<Vector3Int> { new Vector3Int(0, -1, 1) }, ElementsEnum.Pyro);
            ServiceLocator.GetService<GridService>().ApplyElementToTiles(new List<Vector3Int> { new Vector3Int(1, -1, 0) }, ElementsEnum.Hydro);
        }
    }

    public void RegisterToPointerClicked(Action<Vector3> action)
    {
        PointerClicked += action;
    }

    public void DeregisterToPointerClicked(Action<Vector3> action)
    {
        PointerClicked -= action;
    }
}
