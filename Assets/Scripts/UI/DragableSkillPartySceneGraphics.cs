using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Tools;

namespace MVC.View.UI
{
    public abstract class DragableSkillPartySceneGraphics : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Controller.Combat.SkillTypeEnum SkillType;
        [SerializeField] protected Image skillIcon;
        [SerializeField] protected TextMeshProUGUI skillDescription;
        [SerializeField] protected Image dragingImagePrefab;
        [SerializeField] protected CanvasGroup canvasGroup;

        private Image dragingImage;
        protected Transform canvasTransform;

        public Sprite GetSkillIcon()
        {
            return skillIcon.sprite;
        }

        public string GetSkillDescription()
        {
            return skillDescription.text;
        }

        public void SetSkillIcon(Sprite icon)
        {
            skillIcon.sprite = icon;
        }

        public void SetSkillDescription(string description)
        {
            skillDescription.text = description;
        }

        public virtual void UpdateGraphics()
        {

        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if(dragingImage == null)
            {
                return;
            }

            dragingImage.rectTransform.position = eventData.position;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            dragingImage = Instantiate(dragingImagePrefab, canvasTransform);
            dragingImage.rectTransform.position = eventData.position;

            ServiceLocator.GetService<PartySetupSceneGraphics>().RegisterSkillBeingDraged(this);
            SetupAlpha(0.5f);
        }

        public virtual void Setup(Sprite icon, string description, Transform canvasTransform)
        {
            skillIcon.sprite = icon;
            skillDescription.text = description;
            this.canvasTransform = canvasTransform;
        }

        private void SetupAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Destroy(dragingImage.gameObject);
            dragingImage = null;

            SetupAlpha(1f);
        }
    }
}

