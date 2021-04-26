using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade", order = 1)]
    public class Upgrade : ScriptableObject
    {
        public string id;
        public string label;
        public string level0Label;
        public string description;
        public string level0Description;
        public int price;
        public int maxLevels = 1;
        public float priceMultiplicator = 1.5f;
        public UpgradeSlot slot;
        public int startLevel = 0;
    }
}