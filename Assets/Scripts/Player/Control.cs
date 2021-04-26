using GameJolt.API;
using GameJolt.UI;
using TMPro;
using Ui;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Upgrades;

namespace Player
{
    public class Control : MonoBehaviour
    {
        public float speed = 16;
        public int minAngle = -20;
        public int maxAngle = 20;
        public int rotationSpeed = 5;
        public int maxEnergy = 100;
        public RectTransform energyBar;
        public float maxHeightEnergyBar;
        public float energyEfficiency = 1;
        public int score;
        public TextMeshProUGUI scoreLabel;
        public TextMeshProUGUI rankLabel;
        public TextMeshProUGUI[] moneyLabels;
        public TextMeshProUGUI[] gameOverScoreLabels;
        public GameObject guestGameOverScreen;
        public RectTransform heatBar;
        public Sprite warnSprite;
        public Volume volume;
        public float heatSpeedModificator = .1f;
        public float cooldown = .1f;
        public GameObject level1Body;
        public GameObject level2Body;
        public GameObject level3Body;
        public GameObject level4Body;
        public int drillLevel = 1;
        public float lavaTimer;
        public UpgradeStore store;
        public float lavaStrength = 1f;

        private float _angle;
        private bool _pressingLeft;
        private bool _pressingRight;
        private float _energy;
        private bool _dead;
        private int _money;
        private int _rank;
        private float _heat;
        private Vignette _vignette;
        private float _realSpeed;

        private void Awake()
        {
            _energy = maxEnergy;
            _realSpeed = speed;
            volume.profile.TryGet(out _vignette);
        }

        private void Update()
        {
            if (_dead)
            {
                return;
            }

            if (lavaTimer > 0f)
            {
                lavaTimer -= Time.deltaTime;
                _heat += lavaStrength;
            }
            
            CalculateAngle();

            _realSpeed = speed * 1 - (heatSpeedModificator * _heat);
            
            transform.Translate(0, -_realSpeed * Time.deltaTime, 0);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, 0, _angle),
                (rotationSpeed / (float)maxAngle * 15f) * Time.deltaTime
            );

            _energy -= energyEfficiency * Time.deltaTime;

            energyBar.sizeDelta = new Vector2(
                energyBar.sizeDelta.x,
                maxHeightEnergyBar / maxEnergy * _energy
            );

            heatBar.sizeDelta = new Vector2(
                heatBar.sizeDelta.x,
                85f / 100 * _heat
            );

            _heat = Mathf.Clamp(_heat - cooldown * Time.deltaTime, 0, 100);

            // base is 0.451
            if (_heat >= 70)
            {
                _vignette.intensity.value = 0.451f + 0.549f / 30 * (_heat - 70);
                _vignette.color.value = Color.red;
            }
            else
            {
                _vignette.intensity.value = 0.451f;
                _vignette.color.value = Color.black;
            }

            scoreLabel.text = score.ToString();

            foreach (var moneyLabel in moneyLabels)
            {
                moneyLabel.text = $"${_money.ToString()}";
            }

            if (_energy <= 0)
            {
                GameJoltUI.Instance.QueueNotification("You ran out of energy!", warnSprite);
                Die();
            }

            if (_heat >= 100)
            {
                GameJoltUI.Instance.QueueNotification("You overheated!", warnSprite);
                Die();
            }

            #region DEBUG METHODS

            if (Input.GetKeyDown(KeyCode.K))
            {
                GameJoltUI.Instance.QueueNotification("You killed yourself!", warnSprite);
                Die();
            }

            if (Application.isEditor && Input.GetKeyDown(KeyCode.J))
            {
                _heat += 10;
            }

            if (Application.isEditor && Input.GetKeyDown(KeyCode.L))
            {
                GameJoltUI.Instance.ShowSignIn();
            }

            #endregion

            if (score <= 0)
            {
                return;
            }

            Scores.GetRank(score, 618313, (int rank) => { _rank = rank; });

            rankLabel.text = $"Rank {_rank}";

            foreach (var label in gameOverScoreLabels)
            {
                label.text = $"Score: {score} (Rank {_rank})";
            }
        }

        private void CalculateAngle()
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
        }

        public void Die()
        {
            _dead = true;
            store.CloseForever();

            if (GameJoltAPI.Instance.HasSignedInUser)
            {
                Scores.Add(
                    score,
                    score.ToString(),
                    618313,
                    null,
                    (bool success) => { GameJoltUI.Instance.ShowLeaderboards(_ => { GuestGameOver.Restart(); }); }
                );
                return;
            }

            guestGameOverScreen.SetActive(true);
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

        public void AddMoney(int add)
        {
            _money += add;
        }

        public int GetMoney() => _money;

        public void RemoveMoney(int remove)
        {
            _money = Mathf.Clamp(_money - remove, 0, int.MaxValue);
        }

        public void SetHeat(int heat)
        {
            _heat = heat;
        }
    }
}