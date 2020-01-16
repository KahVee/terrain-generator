using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[,] NoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed, bool islandMode, float a, float b)
    {

        float[,] noise = new float[width, height];
        float[,] island = EdgeFlattener.GenerateEdges(width, a, b);

        //Using System.Random instead of UnityEngine.Random as Unity's Random generates new values unpredictably
        System.Random random = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float x = random.Next(-100000, 100000);
            float y = random.Next(-100000, 100000);
            offsets[i] = new Vector2(x, y);
        }

            if (scale == 0)
            {
                scale = 0.00001f;
            }

        float maxValue = 0f;
        float minValue = 0f;

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                float noiseValue = 0;

                for (int i = 0; i < octaves; i++)
                {
                    //lacunarity adds higher frequency noise every octave, persistence decreases amplitudes
                    float perlinX = offsets[i].x + (x / scale) * Mathf.Pow(lacunarity, i);
                    float perlinY = offsets[i].y + (y / scale) * Mathf.Pow(lacunarity, i);
                    noiseValue += (Mathf.PerlinNoise(perlinX, perlinY) * 2 - 1) * Mathf.Pow(persistence, i);                    
                }

                //Updating max and min values for later normalizing them
                if(noiseValue > maxValue) {
                    maxValue = noiseValue;
                } else if (noiseValue < minValue) {
                    minValue = noiseValue;
                }
                noise[x, y] = noiseValue;

            }
        }

        //Normalizing the noise to a range of 0 to 1
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++){
                noise[x, y] = (noise[x, y] - minValue) / (maxValue - minValue);
                if(islandMode) {
                    noise[x,y] = Mathf.Clamp01(noise[x,y] - island[x, y]);
                }
            }
        }
        return noise;
    }

}
