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

        public void Submit()
        {
            gameObject.SetActive(false);
            
            Scores.Add(
                player.score, 
                player.score.ToString(), 
                input.text, 
                618313, 
                null,
                (bool success) =>
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
    }
}