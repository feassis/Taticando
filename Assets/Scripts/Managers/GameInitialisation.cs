using MVC.Controller.Combat;
using MVC.Controller.Graph;
using MVC.Controller.Grid;
using MVC.Controller.Movement;
using MVC.Controller.Unit;
using System.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.Controller.Inicialization
{
    public class GameInitialisation : MonoBehaviour
    {
        [SerializeField] private Button QuitButton;
        [SerializeField] private Button PlayButton;

        private void Awake()
        {
            InitializeServices();
        }

        private async void InitializeServices()
        {
            InitializeGridService();
            InitializingGraphSearch();
            InitializingMovementSystem();
            SceneManagerInitialization();
            PlayButton.onClick.AddListener(OnPlayButtonClicked);
            QuitButton.onClick.AddListener(OnQuitButtonClicked);

            await InitializeUnitLibrary();

            TurnOnButton();
        }

        private async Task InitializeUnitLibrary()
        {
            var unitLivraryService = await UnitLibraryService.Create();
            ServiceLocator.RegisterService<UnitLibraryService>(unitLivraryService);
        }

        private void TurnOnButton()
        {
            QuitButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(true);
        }

        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }

        private void OnPlayButtonClicked()
        {
            ServiceLocator.GetService<SceneService>().OpenPartySetupScene();
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
