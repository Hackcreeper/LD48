using UnityEngine;

namespace Player
{
    public class DrillHead : MonoBehaviour
    {
        public Transform[] targets;
        public Control player;
        public float rotationSpeed = 900;

        private void Update()
        {
            foreach (var target in targets)
            {
                target.Rotate(new Vector3(
                    0,
                    0,
                    rotationSpeed / 8 * player.speed * Time.deltaTime
                ));
            }
        }
    }
}