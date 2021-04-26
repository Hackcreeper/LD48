namespace Upgrades.Handler
{
    public class SideDrillLeftUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "side_drill_left";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            store.miner.sideArmLeftLevel = level;

            switch (level)
            {
                case 1:
                    store.miner.sideArmsLeftLevel1.SetActive(true);
                    break;
                
                case 2:
                    store.miner.sideArmsLeftLevel1.SetActive(false);
                    store.miner.sideArmsLeftLevel2.SetActive(true);
                    break;
                
                case 3:
                    store.miner.sideArmsLeftLevel2.SetActive(false);
                    store.miner.sideArmsLeftLevel3.SetActive(true);
                    break;
                
                case 4:
                    store.miner.sideArmsLeftLevel3.SetActive(false);
                    store.miner.sideArmsLeftLevel4.SetActive(true);
                    break;
            }
        }
    }
}