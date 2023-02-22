using System.Collections.Generic;
using UnityEngine;

namespace MVC.View.VFX
{
    public class GlowHighlight : MonoBehaviour
    {
        Dictionary<Renderer, Material[]> glowMaterialDictionary = new Dictionary<Renderer, Material[]>();
        Dictionary<Renderer, Material[]> originalMaterialDictionary = new Dictionary<Renderer, Material[]>();

        Dictionary<Color, Material> cachedGlowMaterials = new Dictionary<Color, Material>();

        [SerializeField] private Material glowMaterial;

        [SerializeField] private bool isGlowing = false;

        private Color validSpaceColor = Color.green;
        private Color originalGlowColor;

        private const string glowReference = "_GlowColor";

        private void Awake()
        {
            PrepareMaterialDictionaries();
            originalGlowColor = glowMaterial.GetColor(glowReference);
        }

        private void PrepareMaterialDictionaries()
        {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                Material[] originalMaterials = renderer.materials;
                originalMaterialDictionary.Add(renderer, originalMaterials);
                Material[] newMaterials = new Material[renderer.materials.Length];

                for (int i = 0; i < originalMaterials.Length; i++)
                {
                    Material mat = null;

                    if (cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
                    {
                        mat = new Material(glowMaterial);
                        mat.color = originalMaterials[i].color;
                    }

                    newMaterials[i] = mat;
                }

                glowMaterialDictionary.Add(renderer, newMaterials);
            }
        }

        internal void HighlightValidPath()
        {
            if (isGlowing == false)
            {
                return;
            }

            foreach (Renderer renderer in glowMaterialDictionary.Keys)
            {
                foreach (var item in glowMaterialDictionary[renderer])
                {
                    item.SetColor(glowReference, validSpaceColor);
                }

                renderer.materials = glowMaterialDictionary[renderer];
            }
        }

        internal void ResetGlowHighlight(bool forceReset = false)
        {
            if (isGlowing == false && !forceReset)
            {
                return;
            }

            foreach (Renderer renderer in glowMaterialDictionary.Keys)
            {
                foreach (var item in glowMaterialDictionary[renderer])
                {
                    item.SetColor(glowReference, originalGlowColor);
                }

                renderer.materials = glowMaterialDictionary[renderer];
            }
        }

        public void ToggleGlow()
        {
            if (isGlowing == false)
            {
                ResetGlowHighlight(true);
                foreach (Renderer renderer in originalMaterialDictionary.Keys)
                {
                    renderer.materials = glowMaterialDictionary[renderer];
                }
            }
            else
            {
                foreach (Renderer renderer in originalMaterialDictionary.Keys)
                {
                    renderer.materials = originalMaterialDictionary[renderer];
                }
            }

            isGlowing = !isGlowing;
        }

        public void ToggleGlow(bool state)
        {
            if (isGlowing == state)
            {
                return;
            }

            isGlowing = !state;
            ToggleGlow();
        }
    }

}

