using Sdk.Saving;
using UnityEngine;

namespace Menu.Upgrade
{
    public class UpgradeRotation : Upgrade
    {
        private const string NameSave = "Upgrade_Rotation_";
        
        protected override bool TryToUpgrade()
        {
            if (!base.TryToUpgrade()) return false;
            
            Analytics.OnBuy("Upgrade", $"Rotation_{_carData.typeCar}", _previousCost, SavesFacade.TotalTries);
            SavesFacade.SetUpgrade(NameSave + _carData.typeCar, _upgradeLevel);
            Saves.Save();
            
            return true;
        }

        public override void Load()
        {
            _upgradeLevel = SavesFacade.GetUpgrade(NameSave + _carData.typeCar);
            _maxLevel = _carData.maxRotationLevel;
            
            base.Load();
        }
        
        protected override void UpdateCost()
        {
            _previousCost = _cost;
            _cost = _carData.rotationUpgradeCost.GetCost(_upgradeLevel);
            UpdateView();
        }
    }
}