using MVC.Controller.Combat;
using MVC.Controller.Graph;
using MVC.Controller.Grid;
using MVC.Controller.Movement;
using MVC.Controller.Unit;
using Tools;
using UnityEngine;


namespace MVC.Controller.Inicialization
{
    public class GameInitialisation : MonoBehaviour
    {
        private void Awake()
        {
            InitializeGridService();
            InitializingGraphSearch();
            InitializingMovementSystem();
            SceneManagerInitialization();

            ServiceLocator.GetService<SceneService>().OpenTestScene();
        }

        private void InitializeGridService()
        {
            var gridService = new GridService();
            gridService.Setup();

            ServiceLocator.RegisterService<GridService>(gridService);
        }

        private void InitializingGraphSearch()
        {
            var graphSearch = new GraphSearch();
            ServiceLocator.RegisterService<GraphSearch>(graphSearch);
        }

        private void InitializingMovementSystem()
        {
            var movementSystem = new MovementSystem();
            ServiceLocator.RegisterService<MovementSystem>(movementSystem);
        }

        private void SceneManagerInitialization()
        {
            var sceneService = new SceneService();

            ServiceLocator.RegisterService<SceneService>(sceneService);
        }
    }

}
