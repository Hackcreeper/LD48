using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Chunk : MonoBehaviour
    {
        public int x;
        public int y;
        public int chunkSize;
        public int oreSize;
        public MeshFilter oreMesh;
        public MeshFilter backgroundMesh;
        public LayerDefinition layer;
        public bool finished;
        public bool changed;

        public TileDefinition[,] backgroundTiles;
        public TileDefinition[,] oreTiles;

        public void Initialize(int size, int oreFactor, int posX, int posY, LayerDefinition layerDefinition)
        {
            backgroundTiles = new TileDefinition[size, size];
            oreTiles = new TileDefinition[size / oreFactor, size / oreFactor];
            
            chunkSize = size;
            oreSize = oreFactor;
            x = posX;
            y = posY;
            layer = layerDefinition;
        }

        private void Update()
        {
            if (!changed)
            {
                return;
            }
            
            GenerateMesh();
            changed = false;
        }
        
        public void GenerateMesh()
        {
            if (!finished)
            {
                return;
            }
            
            GenerateBackgroundMesh();
            GenerateOreMesh();
        }

        private void GenerateBackgroundMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var uvs = new List<Vector2>();

            var mesh = backgroundMesh.mesh;

            var squares = 0;
            for (var tx = 0; tx < chunkSize; tx++)
            {
                for (var ty = 0; ty < chunkSize; ty++)
                {
                    var tile = backgroundTiles[tx, ty];

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
            var trianglesRuby = new List<int>();
            var uvs = new List<Vector2>();

            var mesh = oreMesh.mesh;

            var squares = 0;
            for (var tx = 0; tx < chunkSize / oreSize; tx++)
            {
                for (var ty = 0; ty < chunkSize / oreSize; ty++)
                {
                    var tile = oreTiles[tx, ty];
                    if (!tile)
                    {
                        continue;
                    }
                    
                    vertices.Add(new Vector3(tx * oreSize, ty * oreSize, 0));
                    vertices.Add(new Vector3(tx * oreSize + oreSize, ty * oreSize, 0));
                    vertices.Add(new Vector3(tx * oreSize + oreSize, ty * oreSize - oreSize, 0));
                    vertices.Add(new Vector3(tx * oreSize, ty * oreSize - oreSize, 0));

                    if (tile.name == "Ruby")
                    {
                        trianglesRuby.Add(squares * 4 + 0);
                        trianglesRuby.Add(squares * 4 + 1);
                        trianglesRuby.Add(squares * 4 + 3);
                        trianglesRuby.Add(squares * 4 + 1);
                        trianglesRuby.Add(squares * 4 + 2);
                        trianglesRuby.Add(squares * 4 + 3);
                    }
                    else
                    {
                        triangles.Add(squares * 4 + 0);
                        triangles.Add(squares * 4 + 1);
                        triangles.Add(squares * 4 + 3);
                        triangles.Add(squares * 4 + 1);
                        triangles.Add(squares * 4 + 2);
                        triangles.Add(squares * 4 + 3);
                    }

                    uvs.Add(new Vector2(tile.mapX, tile.mapY + tile.mapHeight));
                    uvs.Add(new Vector2(tile.mapX + tile.mapWidth, tile.mapY + tile.mapHeight));
                    uvs.Add(new Vector2(tile.mapX + tile.mapWidth, tile.mapY));
                    uvs.Add(new Vector2(tile.mapX, tile.mapY));
                    
                    squares++; 
                }
            }
            
            mesh.Clear();
            mesh.subMeshCount = 2;
            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetTriangles(trianglesRuby.ToArray(), 1);
            mesh.Optimize();
            mesh.RecalculateNormals();
        }
    }
}