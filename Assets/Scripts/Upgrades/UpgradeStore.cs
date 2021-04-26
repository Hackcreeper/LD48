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
        public Control player;
        public DrillHead drillHead;
        public Miner miner;
        public GameObject storeOverlay;
        public GameObject[] hideWhenOpen;

        private bool _open;
        private bool _closedForever;

        private readonly IUpgradeHandler[] _handlers = {
            new RotationAngleUpgradeHandler(),
            new RotationSpeedUpgradeHandler(),
            new HeatShieldUpgradeHandler(),
            new DrillLevelUpgradeHandler(),
            new BigDrillUpgradeHandler(),
            new SideDrillLeftUpgradeHandler(),
            new SideDrillRightUpgradeHandler(),
            new OilBurnerUpgradeHandler()
        };
        
        private void Start()
        {
            foreach (var upgrade in initialUpgrades)
            {
                var slot = GetSlot(upgrade.slot);

                var button = Instantiate(slot.prefab, slot.parent);
                var component = button.GetComponent<UpgradeButton>();
                
                component.upgrade = upgrade;
                component.player = player;
                component.store = this;
            }
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.U) || _closedForever)
            {
                return;
            }
            
            SetOpen(!_open);
        }

        public void Open()
        {
            SetOpen(true);
        }

        public void Close()
        {
            SetOpen(false);
        }

        private void SetOpen(bool open)
        {
            if (_closedForever)
            {
                return;
            }
            
            _open = open;
            
            storeOverlay.SetActive(_open);
            foreach (var hide in hideWhenOpen)
            {
                hide.SetActive(!_open);
            }
            
            Time.timeScale = _open ? 0 : 1;
        }

        private SlotElement GetSlot(UpgradeSlot slot)
        {
            return slots.FirstOrDefault(s => s.slot == slot);
        }

        public IUpgradeHandler GetHandler(Upgrade upgrade)
        {
            return _handlers.FirstOrDefault(handler => handler.GetId() == upgrade.id);
        }

        public void CloseForever()
        {
            Close();
            _closedForever = true;
        }
    }

    [Serializable]
    public struct SlotElement
    {
        public UpgradeSlot slot;
        public RectTransform parent;
        public GameObject prefab;
    }
}