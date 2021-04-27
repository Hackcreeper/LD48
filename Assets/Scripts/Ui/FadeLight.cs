using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class FadeLight : MonoBehaviour
    {
        public float speed = 10f;
        public bool started;
        public Light luffy;
        public float intensity;
        
        private void Update()
        {
            if (!started)
            {
                return;
            }

            luffy.intensity = Mathf.Lerp(
                luffy.intensity,
                intensity,
                speed * Time.deltaTime
            );
        }
    }
}