namespace Upgrades.Handler
{
    public class BigDrillUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "big_drill";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            store.drillHead.UpgradeToBig();
            // TODO: Increase tunnel size
        }
    }
}