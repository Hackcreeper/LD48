using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Level
{
    public class Chunk : MonoBehaviour
    {
        public int x;
        public int y;
        public int chunkSize;

        public TileDefinition[,] Tiles;

        public void Initialize(int chunkSize, int x, int y)
        {
            Tiles = new TileDefinition[chunkSize, chunkSize];
            this.chunkSize = chunkSize;
            this.x = x;
            this.y = y;
        }

        public void GenerateMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            var mesh = GetComponent<MeshFilter>().mesh;

            int squares = 0;
            for (var tx = 0; tx < chunkSize; tx++)
            {
                for (var ty = 0; ty < chunkSize; ty++)
                {
                    var tile = Tiles[tx, ty];
                    
                    vertices.Add(new Vector3(tx, ty, 0));
                    vertices.Add(new Vector3(tx + 1, ty, 0));
                    vertices.Add(new Vector3(tx + 1, ty - 1, 0));
                    vertices.Add(new Vector3(tx, ty - 1, 0));

                    triangles.Add(squares * 4 + 0);
                    triangles.Add(squares * 4 + 1);
                    triangles.Add(squares * 4 + 3);
                    triangles.Add(squares * 4 + 1);
                    triangles.Add(squares * 4 + 2);
                    triangles.Add(squares * 4 + 3);

                    uvs.Add(new Vector2(tile.mapX, tile.mapY));
                    uvs.Add(new Vector2(tile.mapX + tile.mapWidth, tile.mapY));
                    uvs.Add(new Vector2(tile.mapX + tile.mapWidth, tile.mapY + tile.mapHeight));
                    uvs.Add(new Vector2(tile.mapX, tile.mapY + tile.mapHeight));
     
                    squares++;
                }
            }

            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.Optimize();
            mesh.RecalculateNormals();
        }
    }
}