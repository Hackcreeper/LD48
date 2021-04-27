using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class FadeInCanvas : MonoBehaviour
    {
        public float speed = 10f;
        public bool started;
        public CanvasGroup canvas;
        
        private void Update()
        {
            if (!started)
            {
                return;
            }

            canvas.alpha = Mathf.Lerp(
                canvas.alpha,
                1,
                speed * Time.deltaTime
            );
        }
    }
}