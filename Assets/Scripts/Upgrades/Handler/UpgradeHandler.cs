namespace Upgrades.Handler
{
    public interface IUpgradeHandler
    {
        public string GetId();

        public void Handle(Upgrade upgrade, int level, UpgradeStore store);
    }
}