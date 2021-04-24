using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public LayerDefinition[] layers;
    public int chunkSize = 60;

    private float _noiseSeed;
    private readonly Dictionary<string, Chunk> _chunks = new Dictionary<string, Chunk>();

    private void Awake()
    {
        _noiseSeed = Random.Range(-100000.0f, 100000.0f);
    }

    public void Start()
    {
        for (var x = -2; x <= 4; x++)
        {
            for (var y = -1; y <= 1; y++)
            {
                GenerateChunk(x, y);
            }
        }

        // GenerateChunk(-1, 0);
        // GenerateChunk(0, 0);
        // GenerateChunk(1, 0);
        // GenerateChunk(0, 1);
    }

    private void GenerateChunk(int chunkX, int chunkY)
    {
        var chunk = new GameObject("Chunk_" + chunkX + "_" + chunkY);
        chunk.transform.position = new Vector3(chunkX * chunkSize, chunkY * chunkSize);
        
        var chunkComponent = chunk.AddComponent<Chunk>();
        chunkComponent.Initialize(chunkSize);

        _chunks.Add(chunkX + "_" + chunkY, chunkComponent);
        
        var layer = GetLayerByY(chunkY);

        for (var x = 0; x < chunkSize; x++)
        {
            for (var y = 0; y < chunkSize; y++)
            {
                var noise = GetNoise(chunkX * chunkSize + x, chunkY * chunkSize + y, layer.noiseScale);

                var tile = noise > layer.oreThreshold
                    ? layer.GetRandomOreTile(this, chunkX * chunkSize + x, chunkY * chunkSize + y)
                    : layer.GetRandomBaseTile();

                chunkComponent.Tiles[x, y] = tile;

                var instance = Instantiate(
                    tile.tilePrefab,
                    chunk.transform
                );

                instance.transform.localPosition = new Vector3(x, y);
            }
        }
    }

    public TileDefinition GetTileAt(int x, int y)
    {
        var chunkX = Mathf.FloorToInt(x / (float) chunkSize);
        var chunkY = Mathf.FloorToInt(y / (float) chunkSize);

        var withinX = x - (chunkX * chunkSize);
        var withinY = y - (chunkY * chunkSize);

        return _chunks.ContainsKey(chunkX + "_" + chunkY)
            ? _chunks[chunkX + "_" + chunkY].Tiles[withinX, withinY]
            : null;
    }

    private float GetNoise(float x, float y, float scale)
    {
        return Mathf.PerlinNoise(
            (_noiseSeed + x) / scale,
            (_noiseSeed + y) / scale
        );
    }

    private LayerDefinition GetLayerByY(int y)
    {
        // TODO: Implement
        return layers[0];
    }
}