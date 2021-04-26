using Level;
using UnityEngine;

namespace Ui
{
    public class MainMenu : MonoBehaviour
    {
        public MenuGenerator generator;

        private void Start()
        {
            generator.GenerateChunk(0, 1);
        }
    }
}