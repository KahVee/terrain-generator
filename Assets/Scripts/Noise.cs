using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Noise {

    public static float[,] NoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed, bool islandMode, float a, float b) {

        float[,] noise = new float[width, height];
        float[,] vignette = Vignette.GenerateEdges(width, a, b);

        //Finds a random location on the noise
        //Using System.Random instead of UnityEngine.Random as Unity's Random generates new values based on some unknown state of the game (this breaks the predictability of seeds)
        System.Random random = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++) {
            float x = random.Next(-100000, 100000);
            float y = random.Next(-100000, 100000);
            offsets[i] = new Vector2(x, y);
        }

            //Scale must not be 0 as it's used as a divider later
            if (scale == 0) {
                scale = 0.00001f;
            }

        //Helper variables used to normalize the noise
        float maxValue = 0f;
        float minValue = 0f;

        //Generating noise value for each element of the array. 
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {

                float noiseValue = 0;

                for (int i = 0; i < octaves; i++)
                {
                    //lacunarity determines the relative frequency of octaves
                    float perlinX = offsets[i].x + (x / scale) * Mathf.Pow(lacunarity, i);
                    float perlinY = offsets[i].y + (y / scale) * Mathf.Pow(lacunarity, i);

                    //Each octave's value is scaled between -1 and 1 and after which persistence is applied to decrease later octaves' relative amplitudes
                    //The final value for each "pixel" of the noise is the sum of all octaves of noise
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

                //If island mode is active, apply also a vignette around the edges to the noise.
                if(islandMode) {
                    noise[x,y] = Mathf.Clamp01(noise[x,y] - vignette[x, y]);
                }
            }
        }
        return noise;
    }

}
