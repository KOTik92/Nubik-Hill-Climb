using Sdk.Saving;

namespace Menu.Upgrade
{
    public class UpgradeEngine : Upgrade
    {
        private const string NameSave = "Upgrade_Engine_";
        
        protected override bool TryToUpgrade()
        {
            if (!base.TryToUpgrade()) return false;

            Analytics.OnBuy("Upgrade", $"Engine_{_carData.typeCar}", _previousCost, SavesFacade.TotalTries);
            SavesFacade.SetUpgrade(NameSave + _carData.typeCar, _upgradeLevel);
            Saves.Save();
            
            return true;
        }

        public override void Load()
        {
            _upgradeLevel = SavesFacade.GetUpgrade(NameSave + _carData.typeCar);
            _maxLevel = _carData.maxSpeedLevel;

            base.Load();
        }
        
        protected override void UpdateCost()
        {
            _previousCost = _cost;
            _cost = _carData.speedUpgradeCost.GetCost(_upgradeLevel);
            UpdateView();
        }
    }
}
