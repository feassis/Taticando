using MVC.Controller.Combat;
using MVC.Controller.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Tools
{
    public class SceneService
    {
        public void OpenTestScene()
        {
            SceneManager.LoadScene("TilemapTest");
            SetupCombatScene();
        }

        private void SetupCombatScene()
        {
            CombatManagerInitialization();
        }

        private void CleanUpComatScene()
        {
            ServiceLocator.DeregisterService<CombatManager>();
            ServiceLocator.DeregisterService<UnitManager>();
        }

        private void InitilizingUnitManager(CombatManager combatManager)
        {
            var unitManager = UnitManager.Create(combatManager);
            ServiceLocator.RegisterService<UnitManager>(unitManager);
        }

        private void CombatManagerInitialization()
        {
            var combatManager = new CombatManager();
            ServiceLocator.RegisterService<CombatManager>(combatManager);
            InitilizingUnitManager(combatManager);
        }


    }
}

