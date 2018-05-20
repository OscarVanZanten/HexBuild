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
    }
}
