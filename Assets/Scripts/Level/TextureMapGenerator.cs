using System.IO;
using System.Linq;
using UnityEngine;

namespace Level
{
    public class TextureMapGenerator : MonoBehaviour
    {
        public TileDefinition[] tiles;

        public void Generate()
        {
            var texture = new Texture2D(tiles.Length * 64, 64);
            var uvCoords = texture.PackTextures(tiles.Select(tile => tile.texture).ToArray(), 16);
            texture.Apply();

            File.WriteAllBytes(
                Application.dataPath + "/Textures/TextureMap.png",
                texture.EncodeToPNG()
            );

            for (var i = 0; i < uvCoords.Length; i++)
            {
                tiles[i].mapX = uvCoords[i].x;
                tiles[i].mapY = uvCoords[i].y;
                tiles[i].mapWidth = uvCoords[i].width;
                tiles[i].mapHeight = uvCoords[i].height;
            }

            Debug.Log("Texture map generated!");
        }
    }
}