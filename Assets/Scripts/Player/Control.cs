using GameJolt.API;
using GameJolt.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        public int score;
        public TextMeshProUGUI scoreLabel;
        public TextMeshProUGUI rankLabel;

        private float _angle;
        private bool _pressingLeft;
        private bool _pressingRight;
        private float _energy;
        private bool _dead;

        private void Awake()
        {
            _energy = maxEnergy;
        }

        private void Update()
        {
            if (_dead)
            {
                return;
            }
            
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

            scoreLabel.text = score.ToString();

            if (_energy <= 0)
            {
                Die();
            }
            
            if (Application.isEditor && Input.GetKeyDown(KeyCode.K))
            {
                Die();
            }
            
            if (Application.isEditor && Input.GetKeyDown(KeyCode.H))
            {
                GameJoltUI.Instance.QueueNotification("Muhkuh macht die Muhkuh!");
            }
            
            if (Application.isEditor && Input.GetKeyDown(KeyCode.L))
            {
                GameJoltUI.Instance.ShowSignIn();
            }

            if (score <= 0)
            {
                return;
            }
            
            GameJolt.API.Scores.GetRank(score, 618313, (int rank) =>
            {
                rankLabel.text = $"Rank {rank}";
            });
        }

        private void Die()
        {
            _dead = true;

            if (GameJoltAPI.Instance.HasSignedInUser)
            {
                Scores.Add(score, score.ToString(), 618313, null, (bool success) =>
                {
                    GameJoltUI.Instance.ShowLeaderboards(); 
                });
                return;
            }

            var playerName = "Bobby Bauer";
            
            Scores.Add(score, score.ToString(), playerName, 618313, null, (bool success) =>
            {
                GameJoltUI.Instance.ShowLeaderboards(); 
            });
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