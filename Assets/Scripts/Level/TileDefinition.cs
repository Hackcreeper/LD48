using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Tile", menuName = "Levels/Tile", order = 2)]
    public class TileDefinition : ScriptableObject
    {
        public Texture2D texture;
        public bool isOre = true;
        public bool isBackground;
        public float restoreEnergy;
        public int score;
        public int money;
        public bool renderTransparent;

        public float mapX;
        public float mapY;
        public float mapWidth;
        public float mapHeight;
    }
}