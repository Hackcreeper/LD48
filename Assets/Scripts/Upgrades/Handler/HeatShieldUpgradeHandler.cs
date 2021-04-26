namespace Upgrades.Handler
{
    public class HeatShieldUpgradeHandler : IUpgradeHandler
    {
        public string GetId()
        {
            return "heat_shield";
        }

        public void Handle(Upgrade upgrade, int level, UpgradeStore store)
        {
            store.player.cooldown = level switch
            {
                1 => 1.1f,
                2 => 1.4f,
                3 => 1.8f,
                4 => 2.8f,
                _ => store.player.cooldown
            };
        }
    }
}