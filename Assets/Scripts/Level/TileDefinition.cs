using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Tile", menuName = "Levels/Tile", order = 2)]
    public class TileDefinition : ScriptableObject
    {
        public GameObject tilePrefab;
        public int chance;
    }
}