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

            float xoffset = (float)Random.NextDouble();
            float yoffset = (float)Random.NextDouble();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xx = (float)x / width * scale + xoffset;
                    float yy = (float)y / height * scale + yoffset;
                    map[x + y * width] = Mathf.PerlinNoise(xx, yy);
                }
            }
            return map;
        }
    }
}
