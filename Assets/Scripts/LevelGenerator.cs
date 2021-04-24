using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public LayerDefinition[] layers;
    public GameObject chunkPrefab;
    public TileDefinition backgroundTile;
    
    public int chunkSize = 60;
    public Player player;

    private float _noiseSeed;
    private readonly Dictionary<string, Chunk> _chunks = new Dictionary<string, Chunk>();

    private void Awake()
    {
        _noiseSeed = Random.Range(-100000.0f, 100000.0f);
    }
    
    private void Update()
    {
        var chunkX = GetChunkByCoordinate(player.transform.position.x);
        var chunkY = GetChunkByCoordinate(player.transform.position.y);

        var toBeRemoved = new List<string>();
        foreach (var chunk in _chunks.Where(chunk => chunk.Value.y > chunkY + 1))
        {
            Destroy(chunk.Value.gameObject);
            toBeRemoved.Add(chunk.Key);
        }
        
        toBeRemoved.ForEach(chunk => _chunks.Remove(chunk));
        
        GenerateChunk(chunkX, chunkY);
        GenerateChunk(chunkX-1, chunkY);
        GenerateChunk(chunkX+1, chunkY);
        
        GenerateChunk(chunkX, chunkY-1);
        GenerateChunk(chunkX-1, chunkY-1);
        GenerateChunk(chunkX+1, chunkY-1);
    }

    private void GenerateChunk(int chunkX, int chunkY)
    {
        if (_chunks.ContainsKey(chunkX + "_" + chunkY))
        {
            return;
        }
        
        var layer = GetLayerByY(chunkY);

        var chunk = Instantiate(
            chunkPrefab,
            new Vector3(chunkX * chunkSize, chunkY * chunkSize),
            Quaternion.identity
        );
        chunk.name = $"Chunk_{chunkX}_{chunkY}";

        var chunkComponent = chunk.GetComponent<Chunk>();
        chunkComponent.Initialize(chunkSize, chunkX, chunkY, layer);

        _chunks.Add(chunkX + "_" + chunkY, chunkComponent);
        
        for (var x = 0; x < chunkSize; x++)
        {
            for (var y = 0; y < chunkSize; y++)
            {
                var noise = GetNoise(chunkX * chunkSize + x, chunkY * chunkSize + y, layer.noiseScale);

                var tile = noise > layer.oreThreshold
                    ? layer.GetRandomOreTile(this, chunkX * chunkSize + x, chunkY * chunkSize + y)
                    : layer.GetRandomBaseTile();

                chunkComponent.Tiles[x, y] = tile;
            }
        }

        chunkComponent.GenerateMesh();
    }

    private int GetChunkByCoordinate(float coordinate)
    {
        return Mathf.FloorToInt(coordinate / (float) chunkSize);
    }
    
    public TileDefinition GetTileAt(int x, int y)
    {
        var chunkX = GetChunkByCoordinate(x);
        var chunkY = GetChunkByCoordinate(y);

        var withinX = x - (chunkX * chunkSize);
        var withinY = y - (chunkY * chunkSize);

        return _chunks.ContainsKey(chunkX + "_" + chunkY)
            ? _chunks[chunkX + "_" + chunkY].Tiles[withinX, withinY]
            : null;
    }

    public int DestroyTile(int x, int y)
    {
        var tile = GetTileAt(x, y);
        if (!tile || tile.isBackground)
        {
            return 0;
        }
        
        var chunkX = GetChunkByCoordinate(x);
        var chunkY = GetChunkByCoordinate(y);

        var withinX = x - (chunkX * chunkSize);
        var withinY = y - (chunkY * chunkSize);

        if (!_chunks.ContainsKey($"{chunkX}_{chunkY}"))
        {
            return 0;
        }

        _chunks[chunkX + "_" + chunkY].Tiles[withinX, withinY] = backgroundTile;
        _chunks[chunkX + "_" + chunkY].GenerateMesh();

        return 0;
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