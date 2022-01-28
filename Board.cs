using System;
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
        byte enPassantOpportunityLocation;
        bool enPassantOpportunityExistence;
        public byte turn {get; set;} //0 - white, 1 - black
        public bool didEnPassant;
        public Board()
        {
            turn = 0;
            didEnPassant = false;
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

        public void CreateEnPassantOpportunity(byte location)
        {
            enPassantOpportunityLocation = location;
            enPassantOpportunityExistence = true;
        }

        public Move CheckMove(byte src, byte dest)
        {
            byte diff = (byte)(turn == 0 ? dest - src : src - dest);

            if(pawns[turn][src] && !pawns[turn][dest]) //src is a friendly pawn and dest is not with a friendly pawn
            {
                if ((diff == 8 || (src / 8 == (turn == 0 ? 1 : 6) && diff == 16 && !pawns[1 - turn][dest + (turn == 0 ? -8 : 8)])) && !pawns[1 - turn][dest]) //non eating move
                    return new Move(src, dest, false);

                if ((diff == 7 && src % 8 != (turn == 0 ? 0 : 7)) || (diff == 9 && src % 8 != (turn == 0 ? 7 : 0))) //is diagonal
                {
                    if (pawns[1 - turn][dest]) //dest is an enemy
                        return new Move(src, dest, true, dest);
                    if (enPassantOpportunityExistence && dest == enPassantOpportunityLocation) //en passant check
                        return new Move(src, dest, true, (byte)(dest + (turn == 0 ? -8 : 8)));
                }
            }

            return null;
        }

        public List<Move> GetAllPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            Move move;
            for (byte i = 0; i < 64; i++)
            {
                for (byte j = 0; j < 64; j++)
                {
                    move = CheckMove(i, j);
                    if (move != null)
                        moves.Add(move);
                }
            }
            return moves;
        }

        public bool CheckForAnyPossibleMoves()
        {
            for (byte i = 0; i < 64; i++)
            {
                for (byte j = 0; j < 64; j++)
                {
                    if (CheckMove(i, j) != null)
                        return true;
                }
            }
            return false;
        }

        public bool CheckIfPawnAtTheLastRow()
        {
            for(byte i = 0; i < 8; i++)
            {
                if (pawns[1 - turn][(turn == 0 ? i : 63 - i)])
                    return true;
            }
            return false;
        }

        public bool CheckIfMatchEnd()
        {
            return CheckIfPawnAtTheLastRow() || !CheckForAnyPossibleMoves();
        }

        public bool Move(byte src, byte dest)
        {
            Move move = CheckMove(src, dest);
            if (move == null)
            {
                return false;
            }

            pawns[turn][src] = false;
            pawns[turn][dest] = true;

            if (move.didEat)
            {
                pawns[1 - turn][move.eatLocation] = false;
                didEnPassant = move.eatLocation != move.dest;
            }
            else
                didEnPassant = false;

            if ((turn == 0 ? dest - src : src - dest) == 16)
                CreateEnPassantOpportunity((byte)(dest + (turn == 0 ? -8 : 8)));
            else
                enPassantOpportunityExistence = false;

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
