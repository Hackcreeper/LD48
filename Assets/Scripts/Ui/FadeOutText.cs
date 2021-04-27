using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class FadeOutText : MonoBehaviour
    {
        public float speed = 10f;
        public bool started;
        public TextMeshProUGUI text;
        
        private void Update()
        {
            if (!started)
            {
                return;
            }

            text.color = Color.Lerp(
                text.color,
                new Color(text.color.r, text.color.g, text.color.b, 0),
                speed * Time.deltaTime
            );
        }
    }
}