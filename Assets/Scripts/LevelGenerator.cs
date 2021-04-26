using System.Collections.Generic;
using System.Linq;
using Level;
using Player;
using UnityEngine;

public class LevelGenerator : MonoBehaviour 
{
    public LayerDefinition[] layers;
    public TileDefinition backgroundTile;
    public Generator generator;

    public int chunkSize = 60;
    public int oreSize = 3;
    public Control player;
    
    public readonly Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();
    
    private void Update()
    {
        var chunkX = GetChunkByCoordinate(player.transform.position.x);
        var chunkY = GetChunkByCoordinate(player.transform.position.y);

        var toBeRemoved = new List<string>();
        foreach (var chunk in chunks.Where(chunk => chunk.Value.y > chunkY + 1))
        {
            Destroy(chunk.Value.gameObject);
            toBeRemoved.Add(chunk.Key);
        }

        toBeRemoved.ForEach(chunk => chunks.Remove(chunk));

        GenerateChunk(chunkX, chunkY);
        GenerateChunk(chunkX - 1, chunkY);
        GenerateChunk(chunkX + 1, chunkY);

        GenerateChunk(chunkX, chunkY - 1);
        GenerateChunk(chunkX - 1, chunkY - 1);
        GenerateChunk(chunkX + 1, chunkY - 1);
    }

    private void GenerateChunk(int chunkX, int chunkY)
    {
        generator.GenerateChunk(chunkX, chunkY);
    }

    private int GetChunkByCoordinate(float coordinate)
    {
        return Mathf.FloorToInt(coordinate / chunkSize);
    }

    public TileDefinition GetTileAt(int x, int y)
    {
        var chunkX = GetChunkByCoordinate(x);
        var chunkY = GetChunkByCoordinate(y);

        var withinX = x - (chunkX * chunkSize);
        var withinY = y - (chunkY * chunkSize);

        return chunks.ContainsKey(chunkX + "_" + chunkY)
            ? chunks[chunkX + "_" + chunkY].backgroundTiles[withinX, withinY]
            : null;
    }

    private TileDefinition GetOreAt(int x, int y)
    {
        var chunkX = GetChunkByCoordinate(x);
        var chunkY = GetChunkByCoordinate(y);

        var withinX = Mathf.FloorToInt((x - (chunkX * chunkSize)) / (float)oreSize);
        var withinY = Mathf.FloorToInt((y - (chunkY * chunkSize)) / (float)oreSize);

        return chunks.ContainsKey(chunkX + "_" + chunkY)
            ? chunks[chunkX + "_" + chunkY].oreTiles[withinX, withinY]
            : null;
    }

    public FluidDefinition GetFluidAt(int x, int y)
    {
        var chunkX = GetChunkByCoordinate(x);
        var chunkY = GetChunkByCoordinate(y);

        var withinX = Mathf.FloorToInt((x - (chunkX * chunkSize)) / (float)oreSize);
        var withinY = Mathf.FloorToInt((y - (chunkY * chunkSize)) / (float)oreSize);

        return chunks.ContainsKey(chunkX + "_" + chunkY)
            ? chunks[chunkX + "_" + chunkY].fluidTiles[withinX, withinY]
            : null;
    }

    public int DestroyTile(int x, int y)
    {
        var chunkX = GetChunkByCoordinate(x);
        var chunkY = GetChunkByCoordinate(y);
        
        if (!chunks.ContainsKey($"{chunkX}_{chunkY}"))
        {
            return 0;
        }
        
        var removed = RemoveBackground(chunkX, chunkY, x, y);
        var score = RemoveOre(chunkX, chunkY, x, y);

        if (removed || score > 0)
        {
            chunks[chunkX + "_" + chunkY].changed = true;
        }

        return score;
    }

    private bool RemoveBackground(int chunkX, int chunkY, int x, int y)
    {
        var tile = GetTileAt(x, y);
        if (!tile || tile.isBackground)
        {
            return false;
        }

        var withinX = x - (chunkX * chunkSize);
        var withinY = y - (chunkY * chunkSize);

        chunks[chunkX + "_" + chunkY].backgroundTiles[withinX, withinY] = backgroundTile;
        return true;
    }
    
    private int RemoveOre(int chunkX, int chunkY, int x, int y)
    {
        var tile = GetOreAt(x, y);
        if (!tile)
        {
            return 0;
        }

        var withinX = Mathf.FloorToInt((x - (chunkX * chunkSize)) / (float)oreSize);
        var withinY = Mathf.FloorToInt((y - (chunkY * chunkSize)) / (float)oreSize);

        chunks[chunkX + "_" + chunkY].oreTiles[withinX, withinY] = null;

        if (tile.restoreEnergy > 0)
        {
            player.RestoreEnergy(tile.restoreEnergy);
        }

        player.AddMoney(tile.money);

        return tile.score;
    }
    
    public LayerDefinition GetLayerByY(int y)
    {
        var chosen = layers[0];

        foreach (var layer in layers)
        {
            if (layer.startY <= -y)
            {
                chosen = layer;
            }
        }

        return chosen;
    }
}