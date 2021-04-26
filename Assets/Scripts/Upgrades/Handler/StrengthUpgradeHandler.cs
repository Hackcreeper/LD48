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
            switch (level)
            {
                case 1:
                    store.player.speed *= 1.1f;
                    break;
                
                case 2:
                    store.player.speed *= 1.1f;
                    break;

                case 3:
                    store.player.speed *= 1.1f;
                    break;

                case 4:
                    store.player.speed *= 1.1f;
                    break;
            }
        }
    }
}