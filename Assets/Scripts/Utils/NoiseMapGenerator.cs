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

        public static float[] GeneratePerlinNoice(int width, int height, float seed)
        {
            float[] map = new float[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float xx = x * 1.0f + x/seed;
                    float yy = y * 1.0f + y/seed;
                    map[x + y * width] = Mathf.PerlinNoise(xx/ (width* 1.0f), (yy)/(height* 1.0f) );
                }
            }
            return map;
        }

    }
}
