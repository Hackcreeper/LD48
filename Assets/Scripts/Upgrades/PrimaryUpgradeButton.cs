using UnityEngine.UI;

namespace Upgrades
{
    public class PrimaryUpgradeButton : UpgradeButton
    {
        protected override void RerenderUi()
        {
            label.text = $"{upgrade.label}<br><size=16>(Level {_level}/{upgrade.maxLevels})</size>";
            description.text = upgrade.description;
            price.text = $"${GetPrice()}";
            iconImage.sprite = upgrade.icons[_level];
        }
    }
}