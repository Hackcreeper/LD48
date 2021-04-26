using UnityEngine;

namespace Upgrades.Handler
{
    public class StrengthUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "strength";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            Debug.Log("Strength upgrade level " + level);
        }
    }
}