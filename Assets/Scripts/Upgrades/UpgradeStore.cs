using System;
using System.Linq;
using Player;
using UnityEngine;
using Upgrades.Handler;

namespace Upgrades
{
    public class UpgradeStore : MonoBehaviour
    {
        public Upgrade[] initialUpgrades;
        public SlotElement[] slots;
        public GameObject upgradePrefab;
        public Control player;

        private readonly IUpgradeHandler[] _handlers = {
            new StrengthUpgradeHandler(),
            new RotationAngleUpgradeHandler(),
            new RotationSpeedUpgradeHandler(),
            new HeatShieldUpgradeHandler()
        };
        
        private void Start()
        {
            foreach (var upgrade in initialUpgrades)
            {
                var slotParent = GetParentForSlot(upgrade.slot);

                var button = Instantiate(upgradePrefab, slotParent);
                var component = button.GetComponent<UpgradeButton>();
                
                component.upgrade = upgrade;
                component.player = player;
                component.store = this;
            }
        }

        private RectTransform GetParentForSlot(UpgradeSlot slot)
        {
            return (slots.Where(s => s.slot == slot).Select(s => s.parent)).FirstOrDefault();
        }

        public IUpgradeHandler GetHandler(Upgrade upgrade)
        {
            return _handlers.FirstOrDefault(handler => handler.GetId() == upgrade.id);
        }
    }

    [Serializable]
    public struct SlotElement
    {
        public UpgradeSlot slot;
        public RectTransform parent;
    }
}