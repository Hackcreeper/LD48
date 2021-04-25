using System;
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

        private float _angle;
        private bool _pressingLeft;
        private bool _pressingRight;
        
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
    }
}