using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class Chunk : MonoBehaviour
    {
        public int x;
        public int y;
        public int chunkSize;
        public MeshFilter oreMesh;
        public LayerDefinition layer;

        public TileDefinition[,] Tiles;

        public void Initialize(int chunkSize, int posX, int posY, LayerDefinition layerDefinition)
        {
            Tiles = new TileDefinition[chunkSize, chunkSize];
            this.chunkSize = chunkSize;
            x = posX;
            y = posY;
            layer = layerDefinition;
        }

        public void GenerateMesh()
        {
            GenerateBackgroundMesh();
            GenerateOreMesh();
        }

        private void GenerateBackgroundMesh()
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
                    if (tile.isOre)
                    {
                        tile = layer.baseTiles[GetRandomNumberBasedOnCoords(x*chunkSize+tx, y*chunkSize+ty, 0, layer.baseTiles.Length)].tile;
                    }

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
        
        private void GenerateOreMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            var mesh = oreMesh.mesh;

            var squares = 0;
            for (var tx = 0; tx < chunkSize; tx++)
            {
                for (var ty = 0; ty < chunkSize; ty++)
                {
                    var tile = Tiles[tx, ty];
                    if (!tile.isOre)
                    {
                        continue;
                    }

                    vertices.Add(new Vector3(tx, ty, 0));
                    vertices.Add(new Vector3(tx + 1.5f, ty, 0));
                    vertices.Add(new Vector3(tx + 1.5f, ty - 1.5f, 0));
                    vertices.Add(new Vector3(tx, ty - 1.5f, 0));

                    triangles.Add(squares * 4 + 0);
                    triangles.Add(squares * 4 + 1);
                    triangles.Add(squares * 4 + 3);
                    triangles.Add(squares * 4 + 1);
                    triangles.Add(squares * 4 + 2);
                    triangles.Add(squares * 4 + 3);

                    uvs.Add(new Vector2(tile.mapX, tile.mapY + tile.mapHeight));
                    uvs.Add(new Vector2(tile.mapX + tile.mapWidth, tile.mapY + tile.mapHeight));
                    uvs.Add(new Vector2(tile.mapX + tile.mapWidth, tile.mapY));
                    uvs.Add(new Vector2(tile.mapX, tile.mapY));

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

        private int GetRandomNumberBasedOnCoords(int x, int y, int min, int max)
        {
            var seed = BitConverter.ToInt32(BitConverter.GetBytes(
                x * 17 + y
            ), 0);

            return new System.Random(seed).Next(min, max);
        }
    }
}