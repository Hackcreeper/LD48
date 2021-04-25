using System.Collections;
using UnityEngine;

namespace Level
{
    public class Generator : MonoBehaviour
    {
        public LevelGenerator level;
        public GameObject chunkPrefab;
        
        private float _noiseSeed;
        
        private void Awake()
        {
            _noiseSeed = Random.Range(-100000.0f, 100000.0f);
        }

        public void GenerateChunk(int chunkX, int chunkY)
        {
            if (level.chunks.ContainsKey(chunkX + "_" + chunkY))
            {
                return;
            }
            
            var chunk = Instantiate(
                chunkPrefab,
                new Vector3(chunkX * level.chunkSize, chunkY * level.chunkSize),
                Quaternion.identity
            );
            chunk.name = $"Chunk_{chunkX}_{chunkY}";
            
            var layer = level.GetLayerByY(chunkY);
            var chunkComponent = chunk.GetComponent<Chunk>();
            chunkComponent.Initialize(level.chunkSize, level.oreSize, chunkX, chunkY, layer);

            level.chunks.Add(chunkX + "_" + chunkY, chunkComponent);

            var routine = GenerateTiles(chunkComponent);
            StartCoroutine(routine);
        }

        private IEnumerator GenerateTiles(Chunk chunk)
        {
            for (var x = 0; x < level.chunkSize; x++)
            {
                for (var y = 0; y < level.chunkSize; y++)
                {
                    chunk.BackgroundTiles[x, y] = chunk.layer.GetRandomBaseTile();
                }
                yield return null;
            }
            
            for (var x = 0; x < level.chunkSize / level.oreSize; x++)
            {
                for (var y = 0; y < level.chunkSize / level.oreSize; y++)
                {
                    var noise = GetNoise(
                        chunk.x * level.chunkSize + x * level.oreSize, 
                        chunk.y * level.chunkSize + y * level.oreSize,
                        chunk.layer.noiseScale
                    );
            
                    if (noise < chunk.layer.oreThreshold)
                    {
                        continue;
                    }
            
                    var ore = chunk.layer.GetRandomOreTile(level, chunk.x * level.chunkSize + x, chunk.y * level.chunkSize + y);
                    chunk.OreTiles[x, y] = ore;
                }

                yield return null;
            }
            
            chunk.finished = true;
            chunk.GenerateMesh();
        }
        
        private float GetNoise(float x, float y, float scale)
        {
            return Mathf.PerlinNoise(
                (_noiseSeed + x) / scale,
                (_noiseSeed + y) / scale
            );
        }
    }
}