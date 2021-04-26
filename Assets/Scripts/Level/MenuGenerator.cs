using UnityEngine;

namespace Level
{
    public class MenuGenerator : MonoBehaviour
    {
        public LevelGenerator level;
        public GameObject chunkPrefab;
        public LayerDefinition menuLayer;
        public TileDefinition grasTile;

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
            
            var chunkComponent = chunk.GetComponent<Chunk>();
            chunkComponent.Initialize(level.chunkSize, level.oreSize, chunkX, chunkY, menuLayer, level);
            
            level.chunks.Add(chunkX + "_" + chunkY, chunkComponent);
            
            for (var x = 0; x < level.chunkSize; x++)
            {
                for (var y = 0; y < level.chunkSize; y++)
                {
                    var tile = y == level.chunkSize - 1 
                        ? new BackgroundType() { tiles = new []{ new LayerTile() {chance = 1, tile = grasTile} }, minNoise = 0} 
                        : chunkComponent.layer.baseTiles[0];
                    
                    chunkComponent.backgroundTiles[x, y] = chunkComponent.layer.GetRandomBaseTile(tile);
                }
            }
            
            chunkComponent.finished = true;
            chunkComponent.GenerateMesh();
        }
    }
}