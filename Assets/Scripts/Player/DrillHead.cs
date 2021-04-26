using UnityEngine;

namespace Player
{
    public class DrillHead : MonoBehaviour
    {
        public Transform[] targets;
        public float rotationSpeed = 10;
    
        private void Update()
        {
            foreach (var target in targets)
            {
                target.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));   
            }
        }
    }
}