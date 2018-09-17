using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class NoiseMapGenerator
    {
        public static System.Random Random = new System.Random();

        public static float[] GeneratePerlinNoice(int width, int height, float scale)
        {
            float[] map = new float[width * height];

            float xoffset = (float)Random.NextDouble() * UInt16.MaxValue/2;// (float)(Random.Next(int.MinValue, int.MaxValue));
            float yoffset =  (float)Random.NextDouble() * UInt16.MaxValue / 2;  //(float)(Random.Next(int.MinValue, int.MaxValue));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xx = xoffset + (float)x / width  * scale;
                    float yy = yoffset + (float)y / height  * scale ;
                    map[x + y * width] = Mathf.PerlinNoise(xx, yy);
                }
            }
            return map;
        }
    }
}
