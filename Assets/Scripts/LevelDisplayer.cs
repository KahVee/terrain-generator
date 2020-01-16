using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDisplayer : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter[] meshFilter;
    public MeshRenderer[] meshRenderer;

    public void DrawArray(float[,] noise)
    {
        textureRenderer.sharedMaterial.mainTexture = CreateNoiseTexture(noise);
        textureRenderer.transform.localScale = new Vector3(noise.GetLength(0), 1, noise.GetLength(1));
    }

    public Texture2D CreateNoiseTexture(float[,] noise)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float colorValue = noise[x, y];

                colors[y * width + x] = new Color(colorValue, colorValue, colorValue);
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        return texture;
    }

    public void DrawMesh(MeshData data, Texture2D texture, bool flatShading)
    {
        meshFilter[0].sharedMesh = data.CreateMesh(flatShading);
        meshRenderer[0].sharedMaterial.mainTexture = texture;
    }

    public void DrawSea(MeshData[] data, Texture2D texture, int size)
    {
        int scale = size * (int)meshFilter[1].transform.localScale.x;
        float y = meshFilter[0].transform.position.y;
        meshFilter[1].sharedMesh = data[0].CreateMesh(true);
        meshRenderer[1].sharedMaterial.mainTexture = texture;
        meshFilter[1].transform.position = new Vector3(-scale, y, scale);
        meshFilter[2].transform.position = new Vector3(0, y, scale);
        meshFilter[3].transform.position = new Vector3(scale, y, scale);
        meshFilter[4].transform.position = new Vector3(-scale, y, 0);
        meshFilter[5].transform.position = new Vector3(scale, y, 0);
        meshFilter[6].transform.position = new Vector3(-scale, y, -scale);
        meshFilter[7].transform.position = new Vector3(0, y, -scale);
        meshFilter[8].transform.position = new Vector3(scale, y, -scale);

    }

    public Texture2D colorTexture(float[,] noise)
    {
        int width = noise.GetLength(0);
        int height = noise.GetLength(1);
        Texture2D texture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float colorValue = noise[x, y];

                Color newColor = new Color32(175, 215, 210, 255);
                if (colorValue < 0.1)
                {
                    newColor = new Color32(69, 91, 127, 255);
                }
                else if (colorValue < 0.25)
                {
                    newColor = new Color32(152, 190, 190, 255);
                }
                else if (colorValue < 0.3)
                {
                    newColor = new Color32(229, 217, 194, 255);
                }
                else if (colorValue < 0.41)
                {
                    newColor = new Color32(181, 186, 97, 255);
                }
                else if (colorValue < 0.55)
                {
                    newColor = new Color32(124, 141, 76, 255);
                }
                else if (colorValue < 0.60)
                {
                    newColor = new Color32(114, 84, 40, 255);
                }
                else if (colorValue < 0.82)
                {
                    newColor = new Color32(100, 100, 100, 255);
                }
                else
                {
                    newColor = new Color32(230, 230, 230, 255);
                }

                colors[y * width + x] = newColor;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels((Color[])colors);
        texture.Apply();
        return texture;
    }
}
