using MVC.Controller.Combat;

public abstract class Skill
{
    public SkillTypeEnum Type;
    public MVC.View.UI.DragableSkillPartySceneGraphics PartyDragable;
    public PlayerUnit Owner;

    public abstract void UpdateDragable();
}
