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
