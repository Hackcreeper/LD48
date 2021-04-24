using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelLayer", menuName = "Levels/Layer", order = 1)]
    public class LayerDefinition : ScriptableObject
    {
        public LayerTile[] baseTiles;
        public LayerTile[] oreTiles;
        public int startY;

        public float noiseScale = 10f;
        public float oreThreshold = 0.8f;
        public float mixChance = 0.2f;

        public TileDefinition GetRandomBaseTile()
        {
            return baseTiles[0].tile;
        }

        public TileDefinition GetRandomOreTile(LevelGenerator level, int x, int y)
        {
            var neighbours = new List<TileDefinition>();

            var top = level.GetTileAt(x, y - 1);
            if (top && top.isOre)
            {
                neighbours.Add(top);
            }
            
            var bottom = level.GetTileAt(x, y + 1);
            if (bottom && bottom.isOre)
            {
                neighbours.Add(bottom);
            }
            
            var left = level.GetTileAt(x - 1, y);
            if (left && left.isOre)
            {
                neighbours.Add(left);
            }
            
            var right = level.GetTileAt(x + 1, y);
            if (right && right.isOre)
            {
                neighbours.Add(right);
            }

            Debug.Log(neighbours.Count);
            
            if (neighbours.Count == 0 || Random.Range(0f, 1f) < mixChance)
            {
                return GetTrueRandomOre();
            }
            
            return neighbours[Random.Range(0, neighbours.Count)];
        }

        private TileDefinition GetTrueRandomOre()
        {
            var possibilities = new List<TileDefinition>();
            foreach (var layerTile in oreTiles)
            {
                for (var i = 0; i < layerTile.chance; i++)
                {
                    possibilities.Add(layerTile.tile);
                }
            }
            
            return possibilities[Random.Range(0, possibilities.Count)];
        }
    }

    [Serializable]
    public struct LayerTile
    {
        public TileDefinition tile;
        public int chance;
    }
}