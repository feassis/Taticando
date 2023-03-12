using MVC.Controller.Unit;
using MVC.Controller.Combat;

public class AreaOfEffectSkill : Skill
{
    public ActionRangeInfo RangeInfo;

    public AreaOfEffectSkill(SkillTypeEnum type, ActionRangeInfo rangeInfo)
    {
        Type = type;
        RangeInfo = rangeInfo;
    }

    public override void UpdateDragable()
    {
        PartyDragable.UpdateGraphics();
    }
}
