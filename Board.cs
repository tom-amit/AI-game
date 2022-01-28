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

        protected byte[] count;
        byte enPassantOpportunityLocation;
        bool enPassantOpportunityExistence;
        public byte turn {get; set;} //0 - white, 1 - black
        private Stack<Move> moveHistory;

        public Board()
        {
            turn = 0;
            pawns = new BitArray[2];
            pawns[0] = new BitArray(boardSize * boardSize, false);
            pawns[1] = new BitArray(boardSize * boardSize, false);
            count = new byte[2];
            count[0] = 8;
            count[1] = 8;
            moveHistory = new Stack<Move>();
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

        public Move CheckMove(byte src, byte dest, byte player)
        {
            byte diff = (byte)(player == 0 ? dest - src : src - dest);

            if(pawns[player][src] && !pawns[player][dest]) //src is a friendly pawn and dest is not with a friendly pawn
            {
                if ((diff == 8 || (src / 8 == (player == 0 ? 1 : 6) && diff == 16 && !pawns[1 - player][dest + (player == 0 ? -8 : 8)])) && !pawns[1 - player][dest]) //non eating move
                    return new Move(src, dest, false, 0, enPassantOpportunityExistence, enPassantOpportunityLocation);

                if ((diff == 7 && src % 8 != (player == 0 ? 0 : 7)) || (diff == 9 && src % 8 != (player == 0 ? 7 : 0))) //is diagonal
                {
                    if (pawns[1 - player][dest]) //dest is an enemy
                        return new Move(src, dest, true, dest, enPassantOpportunityExistence, enPassantOpportunityLocation);
                    if (enPassantOpportunityExistence && dest == enPassantOpportunityLocation) //en passant check
                        return new Move(src, dest, true, (byte)(dest + (player == 0 ? -8 : 8)), enPassantOpportunityExistence, enPassantOpportunityLocation);
                }
            }

            return null;
        }

        public List<Move> GetAllPossibleMoves(byte player)
        {
            List<Move> moves = new List<Move>();
            Move move;
            for (byte i = 0; i < 64; i++)
            {
                for (byte j = 0; j < 64; j++)
                {
                    move = CheckMove(i, j, player);
                    if (move != null)
                        moves.Add(move);
                }
            }
            return moves;
        }

        public bool CheckForAnyPossibleMoves(byte player)
        {
            for (byte i = 0; i < 64; i++)
            {
                for (byte j = 0; j < 64; j++)
                {
                    if (CheckMove(i, j, player) != null)
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
            return CheckIfPawnAtTheLastRow() || !CheckForAnyPossibleMoves(turn);
        }

        public bool Move(byte src, byte dest)
        {
            Move move = CheckMove(src, dest, turn);
            if (move == null)
            {
                return false;
            }

            return Move(move);
        }

        public bool Move(Move move) //Move with a prechecked move
        {
            if (move == null)
            {
                return false;
            }

            moveHistory.Push(move);

            pawns[turn][move.src] = false;
            pawns[turn][move.dest] = true;

            if (move.didEat)
            {
                pawns[1 - turn][move.eatLocation] = false;
                count[1 - turn]--;
            }

            if ((turn == 0 ? move.dest - move.src : move.src - move.dest) == 16)
                CreateEnPassantOpportunity((byte)(move.dest + (turn == 0 ? -8 : 8)));
            else
                enPassantOpportunityExistence = false;

            turn = (byte)(1 - turn);
            return true;
        }

        public bool UnmakeMove()
        {
            if (moveHistory.Count == 0)
                return false;

            Move move = moveHistory.Pop();

            pawns[1 - turn][move.src] = true;
            pawns[1 - turn][move.dest] = false;
            if (move.didEat)
            {
                pawns[turn][move.eatLocation] = true;
                count[turn]++;
            }
            enPassantOpportunityExistence = move.wasEnPassantOpportunityExistence;
            enPassantOpportunityLocation = move.wasEnPassantOpportunityLocation;
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
