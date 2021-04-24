using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelLayer", menuName = "Levels/Layer", order = 1)]
    public class LayerDefinition : ScriptableObject
    {
        public int startY;
    }
}