using GameJolt.API;
using GameJolt.UI;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ui
{
    public class GuestGameOver : MonoBehaviour
    {
        public TMP_InputField input;
        public Control player;

        private bool _wasFocused;

        public void Submit()
        {
            gameObject.SetActive(false);
            
            Scores.Add(
                player.score, 
                player.score.ToString(), 
                input.text, 
                618785, 
                null,
                success =>
                {
                    GameJoltUI.Instance.ShowLeaderboards((_) =>
                    {
                        Restart();
                    });
                });
        }
        
        public static void Restart()
        {
            SceneManager.LoadScene(0);
        }

        private void Update()
        {
            if (_wasFocused && Input.GetKeyDown(KeyCode.Return))
            {
                Submit();
            }

            _wasFocused = input.isFocused;
        }
    }
}