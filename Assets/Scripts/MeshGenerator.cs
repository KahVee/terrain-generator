using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class MeshGenerator {

    public static MeshData GenerateMesh(float[,] heightmap, float amplitude, AnimationCurve curve, LevelGenerator.PolygonAlignment alignment) {
        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);

        MeshData meshData = new MeshData(width, height);
        int vIndex = 0;

        //Different ways of flipping the triangles in adjacent squares
        Func<int, int, bool> polygonAlignmentCondition = (a, b) => true;
        switch (alignment)
        {
            case LevelGenerator.PolygonAlignment.aligned:
                polygonAlignmentCondition = (a, b) => true;
                break;
            case LevelGenerator.PolygonAlignment.mismatched:
                polygonAlignmentCondition = (a, b) => (a % 2 + b % 2 != 1);
                break;
            case LevelGenerator.PolygonAlignment.shuffled:
                polygonAlignmentCondition = (a, b) => (UnityEngine.Random.value < 0.5f);
                break;
            default: 
                break;
        }

        for (int y = 0; y < height; y++){
            for (int x = 0; x < width; x++){
                float vertHeight = heightmap[x,y];
                if(vertHeight < 0.25) {
                    vertHeight = 0;
                } else {
                    vertHeight = curve.Evaluate(heightmap[x, y]) * amplitude;
                }
                meshData.verts[vIndex] = new Vector3(x, vertHeight, y);
                meshData.uvs[vIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width-1 && y < height -1) {
                    if (polygonAlignmentCondition(x, y)) {
                        meshData.AddTriangle(vIndex, vIndex + width, vIndex + width + 1);
                        meshData.AddTriangle(vIndex + width + 1, vIndex + 1, vIndex);
                    } else {
                        meshData.AddTriangle(vIndex, vIndex + width, vIndex + 1);
                        meshData.AddTriangle(vIndex + width + 1, vIndex + 1, vIndex + width);
                    }
                }

                vIndex++;
            }
        }
        
        return meshData;
    }

    public static MeshData FlatMesh(int size) {
        MeshData meshData = new MeshData(2, 2);
        meshData.verts[0] = new Vector3(0, 0, 0);
        meshData.verts[1] = new Vector3(size, 0, 0);
        meshData.verts[2] = new Vector3(0, 0, size);
        meshData.verts[3] = new Vector3(size, 0, size);

        meshData.tris = new int[] { 0, 2, 3, 3, 1, 0 };
        meshData.uvs[0] = new Vector2(0, 0);
        meshData.uvs[1] = new Vector2(1, 0);
        meshData.uvs[2] = new Vector2(0, 1);
        meshData.uvs[3] = new Vector2(1, 1);

        return meshData;
    }    
}

public class MeshData {
    public Vector3[] verts;
    public int[] tris;
    public Vector2[] uvs;

    int triIndex;

    public MeshData(int width, int height) {
        verts = new Vector3[width * height];
        uvs = new Vector2[width * height];
        tris = new int[(width - 1)*(height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c) {
        tris[triIndex] = a;
        tris[triIndex+1] = b;
        tris[triIndex+2] = c;
        triIndex += 3;
    }

    void FlatVertices() {
        Vector3[] flatVerts = new Vector3[tris.Length];
        Vector2[] flatUvs = new Vector2[tris.Length];
        for (int i = 0; i < tris.Length; i++) {
            flatVerts[i] = verts[tris[i]];
            flatUvs[i] = uvs[tris[i]];
            tris[i] = i;
        }

        verts = flatVerts;
        uvs = flatUvs;
    }

    public Mesh CreateMesh(bool flatShading) {
        Mesh mesh = new Mesh();
        //Makes the polygons flat-shaded
        if (flatShading) {
            FlatVertices();
        }
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}