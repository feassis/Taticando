using UnityEngine;

namespace MVC.View.UI
{
    public class AreaOfEffectSkillPartySceneGraphics : DragableSkillPartySceneGraphics
    {
        public override void Setup(Sprite icon, string description, Transform canvasTransform)
        {
            skillIcon.sprite = icon;
            skillDescription.text = description;
            this.canvasTransform = canvasTransform;
        }
    }
}

