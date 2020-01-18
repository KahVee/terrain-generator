using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    //Parameters are public to make it as simple as possible to change them in the Unity editor.
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
    public enum PolygonAlignment { aligned, mismatched, shuffled };
    public PolygonAlignment polygonAlignment;
    public AnimationCurve meshHeightCurve;
    [Range(0.1f, 10f)]
    public float a, b;
    public bool autoUpdate;
    [HideInInspector]
    public LevelDisplayer display;

    void Start() {
        GenerateMap();
    }

    public void GenerateMap() {
        //Generates noise that will be applied to the mesh
        float[,] noise = Noise.NoiseMap(width, height, noiseScale, octaves, persistence, lacunarity, seed, islandMode, a, b);

        //Finds the displayer object and generates a mesh and texture for it to display.
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

        display.DrawArray(Vignette.GenerateEdges(width, a, b));
    }
    
    //Creates flat sea meshes and calls the display to display them around the main terrain
    public MeshData[] GenerateIslandSurroundings() {
        MeshData[] surroundingMeshes = new MeshData[8];
        for (int i = 0; i < 8; i++) {
            surroundingMeshes[i] = MeshGenerator.FlatMesh(width-1);
        }
        return surroundingMeshes;
    }
}
