using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class FadeOut : MonoBehaviour
    {
        public float speed = 10f;
        public bool started;
        public Image image;
        
        private void Update()
        {
            if (!started)
            {
                return;
            }

            image.color = Color.Lerp(
                image.color,
                new Color(image.color.r, image.color.g, image.color.b, 0),
                speed * Time.deltaTime
            );
        }
    }
}