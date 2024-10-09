using System.Collections.Generic;
using Menu;
using Sdk.Saving;
using UnityEngine;

namespace Skin
{
    public class SwitchSkin : Panel
    {
        [SerializeField] private Transform content;
        [SerializeField] private SwipeScroll swipeScroll;

        private List<SkinItem> _skinItems = new List<SkinItem>();

        private void OnEnable()
        {
            for (int i = 0; i < content.childCount; i++)
                _skinItems.Add(content.GetChild(i).GetComponent<SkinItem>());
            
            foreach (var car in _skinItems)
                car.Selected += Switch;
            
            Load();
        }
        
        private void OnDisable()
        {
            foreach (var car in _skinItems)
                car.Selected -= Switch;
        }
        
        public override void ActivatorPanel(bool isActivate)
        {
            base.ActivatorPanel(isActivate);
            if(isActivate)
                Load();
        }

        private void Load()
        {
            bool isLoad = false;
            
            if (SavesFacade.Skin != "None")
            {
                foreach (var skin in _skinItems)
                {
                    isLoad = skin.Load(SavesFacade.Skin) ? true : isLoad;
                }

                if (!isLoad)
                {
                    _skinItems[0].SelectedSkin();
                }
            }
            else
            {
                _skinItems[0].SelectedSkin();
            }
        }

        private void Switch(SkinItem skinItem)
        {
            skinItem.SetPanel(true);

            for (int i = 0; i < _skinItems.Count; i++)
            {
                if(_skinItems[i].TypeSkin != skinItem.TypeSkin)
                    _skinItems[i].SetPanel(false);
                else
                {
                    swipeScroll.Init(i);
                }
            }

            SavesFacade.Skin = skinItem.TypeSkin.ToString();
            Saves.Save();
        }
    }
}
