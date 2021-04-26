using UnityEngine.UI;

namespace Upgrades
{
    public class PrimaryUpgradeButton : UpgradeButton
    {
        public Image image;

        protected override void RerenderUi()
        {
            label.text = $"{upgrade.label}<br><size=16>(Level {_level}/{upgrade.maxLevels})</size>";
            description.text = upgrade.description;
            price.text = $"${GetPrice()}";
            
            // TODO: Image
        }
    }
}