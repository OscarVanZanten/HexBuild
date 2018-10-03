using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Level
{
    public struct HexLocation: IEquatable<HexLocation>
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public HexLocation(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is HexLocation)
            {
                return this == (HexLocation)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1997189103;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(HexLocation h1, HexLocation h2)
        {
            return (h1.X == h2.X && h1.Y == h2.Y && h1.Z == h2.Z);
        }
        public static bool operator !=(HexLocation h1, HexLocation h2)
        {
            return (h1.X != h2.X || h1.Y != h2.Y || h1.Z != h2.Z);
        }

        public override string ToString()
        {
            return "HexLocation{X: " + X + " Y:" + Y + " Z: " + Z + "}";
        }

        public bool Equals(HexLocation other)
        {
           return this == other;
        }
    }
}
