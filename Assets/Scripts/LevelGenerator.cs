using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int seed;
    [Range(20f, 200f)]
    public float noiseScale;
    [Range(1, 10)]
    public int octaves;
    [Range(1, 10)]
    public float lacunarity;
    [Range(0.001f, 1f)]
    public float persistence;
    [Range (0,150)]
    public float amplitude;
    public bool islandMode;
    public bool flatShading;
    public PolygonAlignment polygonAlignment;
    public AnimationCurve meshHeightCurve;
    [Range(0.1f, 10f)]
    public float a, b;
    public bool autoUpdate;
    [HideInInspector]
    public LevelDisplayer display;

    public void GenerateMap()
    {
        float[,] noise = Noise.NoiseMap(width, height, noiseScale, octaves, persistence, lacunarity, seed, islandMode, a, b);

        display = FindObjectOfType<LevelDisplayer>();
        display.DrawMesh(MeshGenerator.GenerateMesh(noise, amplitude, meshHeightCurve, polygonAlignment), display.colorTexture(noise), flatShading);

        if(islandMode) {
            Color seaColor = new Color32(69, 91, 127, 255);
            Texture2D texture = new Texture2D(2, 2);
            Color[] colors = new Color[4];
            for (int i = 0; i < 4; i++) {
                colors[i] = seaColor;
            }
            texture.filterMode = FilterMode.Point;
            texture.SetPixels(colors);
            texture.Apply();

            MeshData[] surroundingMeshes = GenerateIslandSurroundings();
            display.DrawSea(surroundingMeshes, texture, width-1);
        }

        display.DrawArray(EdgeFlattener.GenerateEdges(width, a, b));
    }

    public MeshData[] GenerateIslandSurroundings() {
        MeshData[] surroundingMeshes = new MeshData[8];
        for (int i = 0; i < 8; i++) {
            surroundingMeshes[i] = MeshGenerator.FlatMesh(width-1);
        }
        return surroundingMeshes;
    }

    public enum PolygonAlignment { aligned, mismatched, shuffled };
}
