using MVC.Controller.Combat;
using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace MVC.View.UI
{
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
                ServiceLocator.GetService<CombatManager>().DamageRandomUnitOfCurrentTeam();
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

}
