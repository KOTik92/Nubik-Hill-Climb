using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Car.AI
{
    [Serializable]
    public class Settings
    {
        [SerializeField] internal float fixedUpdateFrameNumber;
        [SerializeField] internal int movement;
    }
    
    [Serializable]
    public class InputCarAI
    {
        private List<Settings> _settings = new List<Settings>();
        private int _fixedUpdateFrameNumber = 0;
        private bool _ignoreSettings;

        internal void AddFixedUpdateFrameNumber()
        {
            _fixedUpdateFrameNumber++;
        }
        
        internal float GetMovement()
        {
            if (_ignoreSettings)
            {
                return 1;
            }

            if (_settings.Count == 0)
                return 0;
            
            float move = _settings[0].movement;
            
            foreach (var setting in _settings)
            {
                if (setting.fixedUpdateFrameNumber <= _fixedUpdateFrameNumber)
                {
                    move = setting.movement;
                }
            }

            return move;
        }

        internal void SetSettings(List<Settings> settingsList)
        {
            _settings.Clear();
            
            foreach (var setting in settingsList)
            {
                _settings.Add(setting);
            }
        }

        internal void SetIgnoreSettings(bool isIgnore)
        {
            _ignoreSettings = isIgnore;
        }
    }
}
