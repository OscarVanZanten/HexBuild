using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Level
{
    public class HexLocation
    {
        public int Q { get; private set; }
        public int R { get; private set; }

        public HexLocation(int q, int r)
        {
            this.Q = q;
            this.R = r;
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
            hashCode = hashCode * -1521134295 + Q.GetHashCode();
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(HexLocation h1, HexLocation h2)
        {
            return (h1.Q == h2.Q && h1.R == h2.R);
        }
        public static bool operator !=(HexLocation h1, HexLocation h2)
        {
            return (h1.Q != h2.Q || h1.R != h2.R);
        }
    }
}
