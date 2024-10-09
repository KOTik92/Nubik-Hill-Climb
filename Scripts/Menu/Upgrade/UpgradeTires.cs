using Sdk.Saving;

namespace Menu.Upgrade
{
    public class UpgradeTires : Upgrade
    {
        private const string NameSave = "Upgrade_Tires_";
        
        protected override bool TryToUpgrade()
        {
            if (!base.TryToUpgrade()) return false;
            
            Analytics.OnBuy("Upgrade", $"Tires_{_carData.typeCar}", _previousCost, SavesFacade.TotalTries);
            SavesFacade.SetUpgrade(NameSave + _carData.typeCar, _upgradeLevel);
            Saves.Save();
            
            return true;
        }

        public override void Load()
        {
            _upgradeLevel = SavesFacade.GetUpgrade(NameSave + _carData.typeCar);
            _maxLevel = _carData.maxGripLevel;

            base.Load();
        }
        
        protected override void UpdateCost()
        {
            _previousCost = _cost;
            _cost = _carData.gripUpgradeCost.GetCost(_upgradeLevel);
            UpdateView();
        }
    }
}