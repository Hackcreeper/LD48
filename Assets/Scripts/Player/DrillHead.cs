using UnityEngine;

namespace Player
{
    public class DrillHead : MonoBehaviour
    {
        public Transform target;
        public float rotationSpeed = 10;
    
        private void Update()
        {
            target.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
    }
}