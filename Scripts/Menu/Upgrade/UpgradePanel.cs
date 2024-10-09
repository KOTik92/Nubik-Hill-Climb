using UnityEngine;

namespace Menu.Upgrade
{
    public class UpgradePanel : Panel
    {
        [SerializeField] private Upgrade[] upgrades;

        public override void ActivatorPanel(bool isActivate)
        {
            base.ActivatorPanel(isActivate);
        }

        internal void SetCar(CarData carData)
        {
            foreach (var upgrade in upgrades)
                upgrade.SetCar(carData);
        }
    }
}
