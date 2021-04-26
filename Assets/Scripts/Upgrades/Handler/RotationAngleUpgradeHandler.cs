namespace Upgrades.Handler
{
    public class RotationAngleUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "rotation_angle";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            switch (level)
            {
                case 1:
                    store.player.minAngle = -25;
                    store.player.maxAngle = 25;
                    break;
                
                case 2:
                    store.player.minAngle = -35;
                    store.player.maxAngle = 35;
                    break;
                
                case 3:
                    store.player.minAngle = -40;
                    store.player.maxAngle = 40;
                    break;
                
                case 4:
                    store.player.minAngle = -45;
                    store.player.maxAngle = 45;
                    break;
            }
        }
    }
}