using MVC.Controller.Unit;
using MVC.Controller.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService
{
    private List<Skill> playerSkills = new List<Skill>();
    private List<PlayerUnit> availableUnits = new List<PlayerUnit>();

    public PlayerService(UnitLibraryService unitLibraryService)
    {
        playerSkills.Add(new AreaOfEffectSkill(SkillTypeEnum.AreaOfEffect, 
            new ActionRangeInfo(3, MVC.View.Grid.NeighbourhoodType.Radial, 3)));
        playerSkills.Add(new AreaOfEffectSkill(SkillTypeEnum.AreaOfEffect, 
            new ActionRangeInfo(3, MVC.View.Grid.NeighbourhoodType.CenteredSquare, 3)));
        List<MVC.Model.Unit.UnitSetupEntry> unitsEntries = unitLibraryService.GetUnitsDataOfTeam(MVC.Model.Combat.TeamEnum.Player);

        foreach (var unit in unitsEntries)
        {
            var playerUnit = new PlayerUnit();
            playerUnit.RangeInfo = unit.ActionInfo;
            playerUnit.PrimaryElement = unit.PrimaryElement;
            playerUnit.CharacterSprite = unit.CharacterSprite;
            playerUnit.CharacterDescription = unit.CharacterDescription;

            availableUnits.Add(playerUnit);

            var newAreaOfEffectSkill = new AreaOfEffectSkill(SkillTypeEnum.AreaOfEffect, playerUnit.RangeInfo);
            newAreaOfEffectSkill.Owner = playerUnit;
            playerSkills.Add(newAreaOfEffectSkill);
        }
    }

    public List<Skill> GetPlayerSkill()
    {
        return playerSkills;
    }

    public List<PlayerUnit> GetAvailableUnits()
    {
        return availableUnits;
    }

    public Skill GetSkillByUnitAndType(PlayerUnit unit, SkillTypeEnum type)
    {
        var skill = playerSkills.Find(s => s.Owner == unit && s.Type == type);

        return skill;
    }

    public Skill GetSkillByItsDragable(MVC.View.UI.DragableSkillPartySceneGraphics dragable)
    {
        var skill = playerSkills.Find(s => s.PartyDragable == dragable);

        return skill;
    }

    public void SwapDragableSkill(MVC.View.UI.DragableSkillPartySceneGraphics dragable1, 
        MVC.View.UI.DragableSkillPartySceneGraphics dragable2)
    {
        MVC.View.UI.DragableSkillPartySceneGraphics tempDrag = dragable2;

        var skill1 = GetSkillByItsDragable(dragable1);
        var skill2 = GetSkillByItsDragable(dragable2);

        if(skill1.Type != skill2.Type)
        {
            return;
        }

        skill2.PartyDragable = dragable1;
        skill2.PartyDragable.SetSkillIcon(tempDrag.GetSkillIcon());
        skill2.PartyDragable.SetSkillDescription(tempDrag.GetSkillDescription());

        skill1.PartyDragable = tempDrag;
        skill1.PartyDragable.SetSkillIcon(dragable1.GetSkillIcon());
        skill1.PartyDragable.SetSkillDescription(dragable1.GetSkillDescription());

        skill1.UpdateDragable();
        skill2.UpdateDragable();
    }
}

public class PlayerUnit
{
    public ActionRangeInfo RangeInfo;
    public MVC.Model.Elements.ElementsEnum PrimaryElement;
    public Sprite CharacterSprite;
    public string CharacterDescription;
}
