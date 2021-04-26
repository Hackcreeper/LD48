namespace Upgrades.Handler
{
    public class RotationSpeedUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "rotation_speed";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            store.player.rotationSpeed = level switch
            {
                1 => 8,
                2 => 12,
                3 => 15,
                4 => 20,
                _ => store.player.rotationSpeed
            };
        }
    }
}