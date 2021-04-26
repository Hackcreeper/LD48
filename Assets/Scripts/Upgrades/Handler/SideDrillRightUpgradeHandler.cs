namespace Upgrades.Handler
{
    public class SideDrillRightUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "side_drill_right";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            store.miner.sideArmRightLevel = level;

            switch (level)
            {
                case 1:
                    store.miner.sideArmsRightLevel1.SetActive(true);
                    break;
                
                case 2:
                    store.miner.sideArmsRightLevel1.SetActive(false);
                    store.miner.sideArmsRightLevel2.SetActive(true);
                    break;
                
                case 3:
                    store.miner.sideArmsRightLevel2.SetActive(false);
                    store.miner.sideArmsRightLevel3.SetActive(true);
                    break;
                
                case 4:
                    store.miner.sideArmsRightLevel3.SetActive(false);
                    store.miner.sideArmsRightLevel4.SetActive(true);
                    break;
            }
        }
    }
}