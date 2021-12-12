﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static PawnGame.Const;

namespace PawnGame
{
    class Board
    {
        BitArray[] pawns; //0 - white, 1 - black
        public byte turn {get; set;} //0 - white, 1 - black
        public Board()
        {
            turn = 0;
            pawns = new BitArray[2];
            pawns[0] = new BitArray(boardSize * boardSize, false);
            pawns[1] = new BitArray(boardSize * boardSize, false);
        }

        public void SetupBoard()
        {
            for(int i = 0; i < 8; i++)
            {
                pawns[0][8 + i] = true;
                pawns[1][8*6 + i] = true;
            }
        }

        public bool CheckMove(byte src, byte dest)
        {
            if (pawns[turn][src] && (!pawns[turn][dest] && !pawns[1 - turn][dest])
                )
            {
                return true;
            }
            return false;
        } 

        public bool Move(byte src, byte dest)
        {
            if (!CheckMove(src, dest))
            {
                return false;
            }
            pawns[turn][src] = false;
            pawns[turn][dest] = true;
            turn = (byte)(1 - turn);
            return true;
        }

        public BitArray GetWhitePawns()
        {
            return pawns[0];
        }
        public BitArray GetBlackPawns()
        {
            return pawns[1];
        }
    }
}
