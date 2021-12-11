using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static soldiers.Const;

namespace soldiers
{
    class Board
    {
        BitArray whitePawns, blackPawns;
        public Board()
        {
            whitePawns = new BitArray(boardSize * boardSize, false);
            blackPawns = new BitArray(boardSize * boardSize, false);
        }

        public void SetupBoard()
        {
            for(int i = 0; i < 8; i++)
            {
                whitePawns[8 + i] = true;
                blackPawns[8*6 + i] = true;
            }
        }

        public BitArray GetWhitePawns()
        {
            return whitePawns;
        }
        public BitArray GetBlackPawns()
        {
            return blackPawns;
        }
    }
}
