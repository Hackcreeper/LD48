using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Control : MonoBehaviour
    {
        public float speed = 16;
        public LevelGenerator level;
        public int minAngle = -20;
        public int maxAngle = 20;
        public int maxEnergy = 100;
        public RectTransform energyBar;
        public float maxHeightEnergyBar;
        public float energyEfficiency = 1;

        private float _angle;
        private bool _pressingLeft;
        private bool _pressingRight;
        private float _energy;

        private void Awake()
        {
            _energy = maxEnergy;
        }

        private void Update()
        {
            if (!_pressingLeft && !_pressingRight)
            {
                _angle = 0;
            }

            if (_pressingLeft)
            {
                _angle = minAngle;
            }
            
            if (_pressingRight)
            {
                _angle = maxAngle;
            }

            if (_pressingLeft && _pressingRight)
            {
                _angle = 0;
            }
            
            transform.Translate(0, -speed * Time.deltaTime, 0);
            transform.rotation = Quaternion.Euler(0, 0, _angle);

            _energy -= energyEfficiency * Time.deltaTime;

            energyBar.sizeDelta = new Vector2(
                energyBar.sizeDelta.x,
                maxHeightEnergyBar / maxEnergy * _energy
            );
        }
        
        public void MoveLeft(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _pressingLeft = false;
                return;
            }

            _pressingLeft = true;
        }

        public void MoveRight(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _pressingRight = false;
                return;
            }

            _pressingRight = true;
        }

        public void RestoreEnergy(float add)
        {
            _energy = Mathf.Clamp(_energy + add, 0, maxEnergy);
        }
    }
}