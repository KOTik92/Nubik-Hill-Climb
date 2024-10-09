using System.Collections.Generic;
using Sdk.Saving;
using UnityEngine;

namespace Menu.Map
{
    public class SwithMap : Panel
    {
        [SerializeField] private Transform content;
        [SerializeField] private SwipeScroll swipeScroll;

        private List<MapItem> _mapItems = new List<MapItem>();

        private void OnEnable()
        {
            for (int i = 0; i < content.childCount; i++)
                _mapItems.Add(content.GetChild(i).GetComponent<MapItem>());
            
            foreach (var car in _mapItems)
                car.Selected += Switch;
            
            Load();
        }
        
        private void OnDisable()
        {
            foreach (var car in _mapItems)
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
            bool isLoaded = false;
            
            if (SavesFacade.Map != "None")
            {
                foreach (var map in _mapItems)
                    isLoaded = map.Load(SavesFacade.Map) ? true : isLoaded;

                if (!isLoaded)
                    _mapItems[0].SelectMap();
            }
            else
            {
                _mapItems[0].SelectMap();
            }
        }

        private void Switch(MapItem mapItem)
        {
            mapItem.SetPanel(true);

            for (int i = 0; i < _mapItems.Count; i++)
            {
                if(_mapItems[i].TypeMap != mapItem.TypeMap)
                    _mapItems[i].SetPanel(false);
                else
                    swipeScroll.Init(i);
            }
            
            SavesFacade.Map = mapItem.TypeMap.ToString();
            Saves.Save();
        }
    }
}
