using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level
{
    public class Generator : MonoBehaviour
    {
        public LevelGenerator level;
        public GameObject chunkPrefab;
        public TileDefinition tnt;

        private float _noiseSeedBackground;
        private float _noiseSeedOre;

        private void Awake()
        {
            _noiseSeedBackground = Random.Range(-100000.0f, 100000.0f);
            _noiseSeedOre = Random.Range(-100000.0f, 100000.0f);
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
            chunkComponent.Initialize(level.chunkSize, level.oreSize, chunkX, chunkY, layer, level);

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
                    var noise = GetNoise(
                        chunk.x * level.chunkSize + x,
                        chunk.y * level.chunkSize + y,
                        chunk.layer.noiseScale,
                        _noiseSeedBackground
                    );

                    var selected = chunk.layer.baseTiles[0];
                    foreach (var type in chunk.layer.baseTiles)
                    {
                        if (noise >= type.minNoise)
                        {
                            selected = type;
                        }
                    }

                    chunk.backgroundTiles[x, y] = chunk.layer.GetRandomBaseTile(selected);
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
                        chunk.layer.noiseScale,
                        _noiseSeedOre
                    );

                    if (noise < chunk.layer.oreThreshold)
                    {
                        if (noise <= chunk.layer.tntThreshold)
                        {
                            chunk.oreTiles[x, y] = tnt;
                        }
                        
                        continue;
                    }

                    var ore = chunk.layer.GetRandomOreTile(
                        level,
                        chunk.x * level.chunkSize + x,
                        chunk.y * level.chunkSize + y
                    );
                    chunk.oreTiles[x, y] = ore;
                }

                yield return null;
            }

            GenerateFluids(chunk);
            FixTnt(chunk);

            chunk.finished = true;
            chunk.GenerateMesh();
        }

        private void FixTnt(Chunk chunk)
        {
            var tnts = new List<Coords>();
            for (var x = 0; x < chunk.chunkSize / chunk.oreSize; x++)
            {
                for (var y = 0; y < chunk.chunkSize / chunk.oreSize; y++)
                {
                    if (chunk.oreTiles[x, y]?.name != "TNT")
                    {
                        continue;
                    }
                    
                    tnts.Add(new Coords {x = x, y = y});
                    chunk.oreTiles[x, y] = null;
                }   
            }
            
            tnts.Shuffle();

            var count = 0;
            tnts.ForEach(coords =>
            {
                if (count >= chunk.layer.maxTnt)
                {
                    return;
                }
                
                chunk.oreTiles[coords.x, coords.y] = tnt;
                count++;
            });
        }

        private void GenerateFluids(Chunk chunk)
        {
            var possible = chunk.layer.fluids
                .Where(fluidLayer => Random.Range(0, 100) < fluidLayer.chance)
                .ToList();

            FluidDefinition fluid = null;
            var width = 0;
            var height = 0;
            var startX = 0;
            var startY = 0;

            if (possible.Count == 0)
            {
                return;
            }

            var selected = possible[Random.Range(0, possible.Count)];
            width = Random.Range(selected.minSize, selected.maxSize);
            height = Random.Range(selected.minSize, selected.maxSize);
            fluid = selected.fluid;
            startX = Random.Range(0, level.chunkSize / level.oreSize - width - 1);
            startY = Random.Range(0, level.chunkSize / level.oreSize - height - 1);

            if (fluid.name == "Lava")
            {
                var light = new GameObject("Laval_Light");
                light.transform.SetParent(chunk.transform);
                var comp = light.AddComponent<Light>();
                light.transform.localPosition = new Vector3(
                    startX * level.oreSize + width * level.oreSize / 2f + level.oreSize,
                    startY * level.oreSize - height * level.oreSize / 2f + level.oreSize * 2,
                    -3
                );

                comp.color = new Color(0.905f, 0.666f, 0.4666f);
                comp.range = Mathf.Max(width, height) * 4;
                comp.intensity = Mathf.Max(width, height) * 13;
                comp.renderMode = LightRenderMode.ForceVertex;
            }
            
            for (var x = 0; x < level.chunkSize / level.oreSize; x++)
            {
                for (var y = 0; y < level.chunkSize / level.oreSize; y++)
                {
                    if (x <= startX || x > startX + width || y < startY || y > startY + height - 1)
                    {
                        continue;
                    }

                    chunk.fluidTiles[x, y] = fluid;
                    chunk.fluidIndex[x, y] = Random.Range(0, fluid.fullTextures.Length);
                }
            }
        }

        private static float GetNoise(float x, float y, float scale, float seed)
        {
            return Mathf.PerlinNoise(
                (seed + x) / scale,
                (seed + y) / scale
            );
        }

        private struct Coords
        {
            public int x;
            public int y;
        }
    }
}