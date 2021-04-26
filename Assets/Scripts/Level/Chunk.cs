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
        public LevelGenerator level;

        public TileDefinition[,] backgroundTiles;
        public TileDefinition[,] oreTiles;
        public FluidDefinition[,] fluidTiles;
        public int[,] fluidIndex;

        public void Initialize(int size, int oreFactor, int posX, int posY, LayerDefinition layerDefinition,
            LevelGenerator lvl)
        {
            backgroundTiles = new TileDefinition[size, size];
            oreTiles = new TileDefinition[size / oreFactor, size / oreFactor];
            fluidTiles = new FluidDefinition[size / oreFactor, size / oreFactor];
            fluidIndex = new int[size / oreFactor, size / oreFactor];

            chunkSize = size;
            oreSize = oreFactor;
            x = posX;
            y = posY;
            layer = layerDefinition;
            level = lvl;
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
            var trianglesFluid = new List<int>();
            var uvs = new List<Vector2>();

            var mesh = oreMesh.mesh;

            var squares = 0;
            for (var tx = 0; tx < chunkSize / oreSize; tx++)
            {
                for (var ty = 0; ty < chunkSize / oreSize; ty++)
                {
                    var tile = oreTiles[tx, ty];
                    var fluid = fluidTiles[tx, ty];

                    if (!tile && !fluid)
                    {
                        continue;
                    }

                    vertices.Add(new Vector3(tx * oreSize, ty * oreSize, 0));
                    vertices.Add(new Vector3(tx * oreSize + oreSize, ty * oreSize, 0));
                    vertices.Add(new Vector3(tx * oreSize + oreSize, ty * oreSize - oreSize, 0));
                    vertices.Add(new Vector3(tx * oreSize, ty * oreSize - oreSize, 0));

                    if (fluid)
                    {
                        trianglesFluid.Add(squares * 4 + 0);
                        trianglesFluid.Add(squares * 4 + 1);
                        trianglesFluid.Add(squares * 4 + 3);
                        trianglesFluid.Add(squares * 4 + 1);
                        trianglesFluid.Add(squares * 4 + 2);
                        trianglesFluid.Add(squares * 4 + 3);

                        // If left is no fluid and top is no fluid
                        var left = level.GetFluidAt(
                            x * chunkSize + tx * oreSize - oreSize,
                            y * chunkSize + ty * oreSize
                        );

                        var right = level.GetFluidAt(
                            x * chunkSize + tx * oreSize + oreSize,
                            y * chunkSize + ty * oreSize
                        );

                        var top = level.GetFluidAt(
                            x * chunkSize + tx * oreSize,
                            y * chunkSize + ty * oreSize + oreSize
                        );
                        
                        var bottom = level.GetFluidAt(
                            x * chunkSize + tx * oreSize,
                            y * chunkSize + ty * oreSize - oreSize
                        );

                        if (!top && !left)
                        {
                            uvs.Add(new Vector2(fluid.mapXTopLeft, fluid.mapYTopLeft + fluid.mapHeight));
                            uvs.Add(
                                new Vector2(fluid.mapXTopLeft + fluid.mapWidth, fluid.mapYTopLeft + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXTopLeft + fluid.mapWidth, fluid.mapYTopLeft));
                            uvs.Add(new Vector2(fluid.mapXTopLeft, fluid.mapYTopLeft));
                        }
                        else if (!top && !right)
                        {
                            uvs.Add(new Vector2(fluid.mapXTopRight, fluid.mapYTopRight + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXTopRight + fluid.mapWidth,
                                fluid.mapYTopRight + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXTopRight + fluid.mapWidth, fluid.mapYTopRight));
                            uvs.Add(new Vector2(fluid.mapXTopRight, fluid.mapYTopRight));
                        }
                        else if (!bottom && !left)
                        {
                            uvs.Add(new Vector2(fluid.mapXBottomLeft, fluid.mapYBottomLeft + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXBottomLeft + fluid.mapWidth,
                                fluid.mapYBottomLeft + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXBottomLeft + fluid.mapWidth, fluid.mapYBottomLeft));
                            uvs.Add(new Vector2(fluid.mapXBottomLeft, fluid.mapYBottomLeft));
                        }
                        else if (!bottom && !right)
                        {
                            uvs.Add(new Vector2(fluid.mapXBottomRight, fluid.mapYBottomRight + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXBottomRight + fluid.mapWidth,
                                fluid.mapYBottomRight + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXBottomRight + fluid.mapWidth, fluid.mapYBottomRight));
                            uvs.Add(new Vector2(fluid.mapXBottomRight, fluid.mapYBottomRight));
                        }
                        else
                        {
                            uvs.Add(new Vector2(fluid.mapXFull[fluidIndex[tx, ty]], fluid.mapYFull[fluidIndex[tx, ty]] + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXFull[fluidIndex[tx, ty]] + fluid.mapWidth, fluid.mapYFull[fluidIndex[tx, ty]] + fluid.mapHeight));
                            uvs.Add(new Vector2(fluid.mapXFull[fluidIndex[tx, ty]] + fluid.mapWidth, fluid.mapYFull[fluidIndex[tx, ty]]));
                            uvs.Add(new Vector2(fluid.mapXFull[fluidIndex[tx, ty]], fluid.mapYFull[fluidIndex[tx, ty]]));
                        }
                    }
                    else
                    {
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
                    }

                    squares++;
                }
            }

            mesh.Clear();
            mesh.subMeshCount = 2;
            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetTriangles(trianglesFluid.ToArray(), 1);
            mesh.Optimize();
            mesh.RecalculateNormals();
        }
    }
}