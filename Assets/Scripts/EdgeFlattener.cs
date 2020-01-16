using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EdgeFlattener
{
    public static float[,] GenerateEdges(int size, float a, float b) {
        float[,] edgeArray = new float[size, size];
        float sqrt2 = Mathf.Sqrt(2);
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {

                float x = i / (float)size * 2 - 1;
                float y = j / (float)size * 2 - 1;

                //square edges
                //float k = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                //circular edges
                float k = Mathf.Sqrt(x * x + y * y) / Mathf.Sqrt(2);
                edgeArray[i, j] = Calculate(k, a, b);
            }
        }
        return edgeArray;
    }

    static float Calculate(float value, float a, float b) {
        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }

}
