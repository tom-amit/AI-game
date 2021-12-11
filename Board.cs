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
        BitArray[,] bitBoard;
        public Board()
        {
            bitBoard = new BitArray[boardSize, boardSize];

        }
    }
}
