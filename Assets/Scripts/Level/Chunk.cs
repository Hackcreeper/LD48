using UnityEngine;

namespace Level
{
    public class Chunk : MonoBehaviour
    {
        public int x;
        public int y;

        public TileDefinition[,] Tiles;

        public void Initialize(int chunkSize)
        {
            Tiles = new TileDefinition[chunkSize, chunkSize];
        }
    }
}