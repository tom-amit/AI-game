using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PawnGame
{
    class Move
    {
        public byte src;
        public byte dest;
        public byte eatLocation;
        public bool didEat;
        public Move(byte src, byte dest, bool didEat, byte eatLocation = 0)
        {
            this.src = src;
            this.dest = dest;
            this.eatLocation = eatLocation;
            this.didEat = didEat;
        }
    }
}
