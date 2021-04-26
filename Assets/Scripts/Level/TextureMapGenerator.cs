using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Level
{
    public class TextureMapGenerator : MonoBehaviour
    {
        public TileDefinition[] tiles;
        public FluidDefinition[] fluids;

        public void Generate()
        {
            GenerateTiles();
            GenerateFluids();

            Debug.Log("Texture map generated!");
        }

        private void GenerateTiles()
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
        }
        
        private void GenerateFluids()
        {
            var texture = new Texture2D(tiles.Length * 64, 64);

            var textures = new List<Texture2D>();
            foreach (var fluid in fluids)
            {
                textures.Add(fluid.topLeftEdgeTexture);
                textures.Add(fluid.topRightEdgeTexture);
                textures.Add(fluid.bottomLeftEdgeTexture);
                textures.Add(fluid.bottomRightEdgeTexture);
                textures.AddRange(fluid.fullTextures);
            }
            
            var uvCoords = texture.PackTextures(textures.ToArray(), 16);
            texture.Apply();

            File.WriteAllBytes(
                Application.dataPath + "/Textures/TextureMapFluids.png",
                texture.EncodeToPNG()
            );

            var i = 0;
            foreach (var fluid in fluids)
            {
                fluid.mapXTopLeft = uvCoords[i].x;
                fluid.mapYTopLeft = uvCoords[i].y;
                
                fluid.mapXTopRight = uvCoords[i+1].x;
                fluid.mapYTopRight = uvCoords[i+1].y;
                
                fluid.mapXBottomLeft = uvCoords[i+2].x;
                fluid.mapYBottomLeft = uvCoords[i+2].y;
                
                fluid.mapXBottomRight = uvCoords[i+3].x;
                fluid.mapYBottomRight = uvCoords[i+3].y;

                fluid.mapXFull = new float[fluid.fullTextures.Length];
                fluid.mapYFull = new float[fluid.fullTextures.Length];
                for (var txt = 0; txt < fluid.fullTextures.Length; txt++)
                {
                    fluid.mapXFull[txt] = uvCoords[i + 4 + txt].x;
                    fluid.mapYFull[txt] = uvCoords[i + 4 + txt].y;
                }
                
                fluid.mapWidth = uvCoords[i].width;
                fluid.mapHeight = uvCoords[i].height;

                i += 4 + fluid.fullTextures.Length;
            }
        }
    }
}