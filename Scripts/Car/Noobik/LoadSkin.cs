using System;
using Sdk.Saving;
using UnityEngine;

namespace Skin
{
    [Serializable]
    public class PlayerRenderer
    {
        public Renderer head;
        public Renderer headWear;
        [Header("----------")]
        public Renderer body;
        public Renderer jacket;
        [Header("----------")]
        public Renderer leftArm;
        public Renderer leftSleve;
        [Header("----------")]
        public Renderer rightArm;
        public Renderer rightSleve;
        [Header("----------")]
        public Renderer leftLeg;
        public Renderer leftPants;
        [Header("----------")]
        public Renderer rightLeg;
        public Renderer rightPants;
    }
    
    [Serializable]
    public class Skins
    {
        public TypeSkin typeSkin;
        [Space] 
        public Material material;
    }
    
    public class LoadSkin : MonoBehaviour
    {
        [SerializeField] private bool isLoadStart;
        [SerializeField] private PlayerRenderer playerRenderer;
        [SerializeField] private Skins[] skinsArray;

        private void Awake()
        {
            if (isLoadStart)
                SetSkin(SavesFacade.Skin);
        }

        internal void SetSkin(string typeSkin)
        {
            if (typeSkin != "None")
            {
                for (int i = 0; i < skinsArray.Length; i++)
                {
                    if (skinsArray[i].typeSkin.ToString() == typeSkin)
                    {
                        SwitchSkin(i);
                        break;
                    }
                }
            }
            else
                SwitchSkin(0);
        }

        private void SwitchSkin(int num)
        {
            playerRenderer.head.material = skinsArray[num].material;
            playerRenderer.headWear.material = skinsArray[num].material;
            playerRenderer.body.material = skinsArray[num].material;
            playerRenderer.jacket.material = skinsArray[num].material;
            playerRenderer.leftArm.material = skinsArray[num].material;
            playerRenderer.leftSleve.material = skinsArray[num].material;
            playerRenderer.rightArm.material = skinsArray[num].material;
            playerRenderer.rightSleve.material = skinsArray[num].material;
            playerRenderer.leftLeg.material = skinsArray[num].material;
            playerRenderer.leftPants.material = skinsArray[num].material;
            playerRenderer.rightLeg.material = skinsArray[num].material;
            playerRenderer.rightPants.material = skinsArray[num].material;
        }
    }
}
