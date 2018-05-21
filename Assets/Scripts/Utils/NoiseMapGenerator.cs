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

        public static float[] GeneratePerlinNoice(int width, int height, float seed, float scale)
        {
            float[] map = new float[width * height];

            float offset = (width + height) / (seed * seed);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xx = (x  * seed)*scale;
                    float yy = (y * seed)*scale;
                    map[x + y * width] = Mathf.PerlinNoise(xx,  yy );
                }
            }
            return map;
        }

    }
}
