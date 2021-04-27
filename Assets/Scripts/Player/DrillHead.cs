using UnityEngine;

namespace Player
{
    public class DrillHead : MonoBehaviour
    {
        public Transform[] targets;
        public Control player;
        public float rotationSpeed = 900;
        public GameObject[] smallHeads;
        public GameObject[] bigHeads;
        public bool isBig;

        private void Update()
        {
            if (player.mainMenu.introRunning)
            {
                return;
            }
            
            foreach (var target in targets)
            {
                target.Rotate(new Vector3(
                    0,
                    0,
                    rotationSpeed / 8 * player.speed * Time.deltaTime
                ));
            }
        }

        public void UpgradeToBig()
        {
            isBig = true;
            
            foreach (var head in smallHeads)
            {
                head.SetActive(false);
            }
            
            foreach (var head in bigHeads)
            {
                head.SetActive(true);
            }
        }
    }
}