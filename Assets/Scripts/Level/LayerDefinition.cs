using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelLayer", menuName = "Levels/Layer", order = 1)]
    public class LayerDefinition : ScriptableObject
    {
        public BackgroundType[] baseTiles;
        public LayerTile[] oreTiles;
        public FluidTile[] fluids;
        public int startY;

        public float noiseScale = 10f;
        public float oreThreshold = 0.8f;
        public float tntThreshold = 0.1f;
        public int maxTnt = 1;

        public TileDefinition GetRandomBaseTile(BackgroundType type)
        {
            var possibilities = new List<TileDefinition>();
            foreach (var layerTile in type.tiles)
            {
                for (var i = 0; i < layerTile.chance; i++)
                {
                    possibilities.Add(layerTile.tile);
                }
            }
            
            return possibilities[Random.Range(0, possibilities.Count)];
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
            
            return neighbours.Count == 0 ? GetTrueRandomOre() : neighbours[Random.Range(0, neighbours.Count)];
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
    public struct BackgroundType
    {
        public LayerTile[] tiles;
        public float minNoise;
    }
    
    [Serializable]
    public struct LayerTile
    {
        public TileDefinition tile;
        public int chance;
    }
    
    [Serializable]
    public struct FluidTile
    {
        public FluidDefinition fluid;
        public int chance;
        public int minSize;
        public int maxSize;
    }
}