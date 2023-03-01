using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.View.UI
{
    public class PartySetupSceneGraphics : MonoBehaviour
    {
        [SerializeField] private Button goToCombatSceneButton;
        [SerializeField] private List<PartySelectionCharacterGraphics> Characters;

        private void Awake()
        {
            goToCombatSceneButton.onClick.AddListener(OnGoToComatSceneButtonClicked);
            SetupCharacters();
        }

        private void OnGoToComatSceneButtonClicked()
        {
            ServiceLocator.GetService<SceneService>().OpenTestScene();
        }

        private void SetupCharacters()
        {
            var unitLibraryService = ServiceLocator.GetService<UnitLibraryService>();
            var units = unitLibraryService.GetUnitsDataOfTeam(Model.Combat.TeamEnum.Player);

            for (int i = 0; i < units.Count; i++)
            {
                Characters[i].Setup(units[i].CharacterSprite, 
                    units[i].PrimaryElement, units[i].CharacterDescription,
                    units[i].ActionInfo);
            }
        }
    }
}


