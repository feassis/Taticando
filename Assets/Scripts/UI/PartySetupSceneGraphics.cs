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
        [SerializeField] private Transform skillScrollHolder;
        [SerializeField] private List<DragableSkillPartySceneGraphics> dragablePrefabs;

        private List<DragableSkillPartySceneGraphics> skillScrollList = new List<DragableSkillPartySceneGraphics>();

        private DragableSkillPartySceneGraphics skillBeingDraged;

        public DragableSkillPartySceneGraphics GetSkillBeingDraged()
        {
            return skillBeingDraged;
        }

        private void Awake()
        {
            goToCombatSceneButton.onClick.AddListener(OnGoToComatSceneButtonClicked);
            SetupCharacters();
            SetupSkillList();
            ServiceLocator.RegisterService<PartySetupSceneGraphics>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.DeregisterService<PartySetupSceneGraphics>();
        }

        public void RegisterSkillBeingDraged(DragableSkillPartySceneGraphics skill)
        {
            skillBeingDraged = skill;
        }

        public void DeregisterSkillBeingDraged()
        {
            skillBeingDraged = null;
        }

        private void OnGoToComatSceneButtonClicked()
        {
            ServiceLocator.GetService<SceneService>().OpenTestScene();
        }

        private void SetupCharacters()
        {
            var playerService = ServiceLocator.GetService<PlayerService>();
            var units = playerService.GetAvailableUnits();

            for (int i = 0; i < units.Count; i++)
            {
                Characters[i].Setup(units[i], transform);
            }
        }

        private void SetupSkillList()
        {
            var playerService = ServiceLocator.GetService<PlayerService>();
            var skillList = playerService.GetPlayerSkill();

            foreach (var skill in skillList)
            {
                if(skill.Owner != null)
                {
                    continue;
                }
                var prefab = GetCorrectDragablePrefab(skill.Type);
                var skillDragable = Instantiate(prefab, skillScrollHolder);
                skillDragable.Setup(null,
                    $" Skill {skill.Type} area {((AreaOfEffectSkill)skill).RangeInfo.NeighbourhoodType}", transform);
                skill.PartyDragable = skillDragable;
            }
        }

        private DragableSkillPartySceneGraphics GetCorrectDragablePrefab(Controller.Combat.SkillTypeEnum type)
        {
            return dragablePrefabs.Find(d => d.SkillType == type);
        }
    }
}


