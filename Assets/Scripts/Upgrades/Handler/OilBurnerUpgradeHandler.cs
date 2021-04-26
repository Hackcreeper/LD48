using Player;

namespace Upgrades.Handler
{
    public class OilBurnerUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "oil_burner";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            store.miner.AddOilTank();
        }
    }
}