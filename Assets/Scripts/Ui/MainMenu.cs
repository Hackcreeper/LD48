using Level;
using Player;
using UnityEngine;
using Upgrades;

namespace Ui
{
    public class MainMenu : MonoBehaviour
    {
        public MenuGenerator generator;
        public bool introRunning = true;
        public bool introFinished;
        public GameObject[] hiddenUiElements;
        public bool started;
        public Control player;
        public FadeOutSprite logo;
        public FadeOutText pressKeyInfo;
        public FadeInText[] fadeInTexts;
        public FadeInCanvas[] fadeInCanvases;
        public FadeLight[] fadeLights;
        public UpgradeStore store;
        
        /**
         * 0 = Player moves right a few meters
         * 1 = Player moves down and mines. Fade in stuff
         */
        public int step;

        private float _timer;

        private void Start()
        {
            generator.GenerateChunk(0, 1);
            generator.GenerateChunk(1, 1);
            generator.GenerateChunk(-1, 1);
        }

        private void Update()
        {
            if (Input.anyKeyDown && !started)
            {
                _timer = 1.5f;
                logo.started = true;
                pressKeyInfo.started = true;
                started = true;
            }

            if (!started)
            {
                return;
            }

            if (step == 0)
            {
                player.transform.Translate(0, -10 * Time.deltaTime, 0);
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    introRunning = false;
                    _timer = 2f;
                    
                    step++;
                }
                
                return;
            }

            if (step == 1)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    foreach (var fadeInText in fadeInTexts)
                    {
                        fadeInText.started = true;
                    }
                    
                    foreach (var fadeInCanvas in fadeInCanvases)
                    {
                        fadeInCanvas.started = true;
                    }
                    
                    foreach (var fadeLight in fadeLights)
                    {
                        fadeLight.started = true;
                    }

                    store.Reopen();
                    
                    introFinished = true;
                    started = false;
                }
            }
            
            // Set light to 0.1
            // Set vignette intensity to 0.451
            // Fade out "press any key to start"
            // Show UI elements
            // Set player light intensity to 40
        }
    }
}