using UnityEngine;
using UnityEngine.UI;

namespace ImgBloom
{
    public class ImageBloom : Image
    {
        #if UNITY_EDITOR

        private static Material additiveMaterial;

        protected override void OnValidate()
        {
            base.OnValidate();
            raycastTarget = false;

            if (additiveMaterial == null)
            {
                additiveMaterial = Resources.Load<Material>("ImgBloom/Material/UI_Additive");
            }

            if (additiveMaterial == null)
            {
                Debug.LogError("ImgBloom/Material/UI_Additive is not found");
                return;
            }

            if (material == additiveMaterial)
            {
                return;
            }
            
            m_Material = additiveMaterial;
        }
        
        #endif
    }
}
