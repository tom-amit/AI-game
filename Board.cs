using System.Collections;
using System.Collections.Generic;
using static PawnGame.Const;

namespace PawnGame
{
    class Board
    {
        BitArray[] pawns; //0 - white, 1 - black

        protected int[] count;
        protected int[] distanceSum;

        byte enPassantOpportunityLocation;
        bool enPassantOpportunityExistence;

        protected int turnsCount;

        public byte turn { get; set; } //0 - white, 1 - black
        private Stack<Move> moveHistory;

        public Board()
        {
            turn = 0;
            turnsCount = 0;
            pawns = new BitArray[2];
            pawns[0] = new BitArray(boardSize * boardSize, false);
            pawns[1] = new BitArray(boardSize * boardSize, false);
            count = new int[2];
            count[0] = 0;
            count[1] = 0;
            distanceSum = new int[2];
            distanceSum[0] = 0;
            distanceSum[1] = 0;
            moveHistory = new Stack<Move>();
        }

        public void SetupBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                pawns[0][8 + i] = true;
                pawns[1][8 * 6 + i] = true;
            }
            count[0] = 8;
            count[1] = 8;
            distanceSum[0] = 8;
            distanceSum[1] = 48;
        }

        public void SetupAddPiece(byte location, byte player)
        {
            pawns[player][location] = true;
            count[player]++;
            distanceSum[player] += location / 8;
        }

        public void CreateEnPassantOpportunity(byte location)
        {
            enPassantOpportunityLocation = location;
            enPassantOpportunityExistence = true;
        }

        public Move CheckMove(byte src, byte dest, byte player)
        {
            byte diff = (byte)(player == 0 ? dest - src : src - dest);

            if (pawns[player][src] && !pawns[player][dest]) //src is a friendly pawn and dest is not with a friendly pawn
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
            for (byte i = 8; i < 56; i++)
            {
                if (pawns[player][i])
                {
                    move = CheckMove(i, (byte)(i + (turn == 0 ? 8 : -8)), player);
                    if (move != null)
                        moves.Add(move);
                    if (i + (turn == 0 ? 16 : -16) < 64 && i + (turn == 0 ? 16 : -16) >= 0)
                    {
                        move = CheckMove(i, (byte)(i + (turn == 0 ? 16 : -16)), player);
                        if (move != null)
                            moves.Add(move);
                    }
                    if (i + (turn == 0 ? 9 : -9) < 64 && i + (turn == 0 ? 9 : -9) >= 0)
                    {
                        move = CheckMove(i, (byte)(i + (turn == 0 ? 9 : -9)), player);
                        if (move != null)
                            moves.Add(move);
                    }
                    move = CheckMove(i, (byte)(i + (turn == 0 ? 7 : -7)), player);
                    if (move != null)
                        moves.Add(move);
                }
            }
            return moves;
        }

        public bool CheckForAnyPossibleMoves(byte player)
        {
            for (byte i = 8; i < 56; i++)
            {
                if (pawns[player][i])
                {
                    if (CheckMove(i, (byte)(i + (turn == 0 ? 8 : -8)), player) != null ||
                        (i + (turn == 0 ? 16 : -16) < 64 && i + (turn == 0 ? 16 : -16) >= 0 && CheckMove(i, (byte)(i + (turn == 0 ? 16 : -16)), player) != null) ||
                        (i + (turn == 0 ? 9 : -9) < 64 && i + (turn == 0 ? 9 : -9) >= 0 && CheckMove(i, (byte)(i + (turn == 0 ? 9 : -9)), player) != null) ||
                        CheckMove(i, (byte)(i + (turn == 0 ? 7 : -7)), player) != null)
                        return true;
                }
            }
            return false;
        }

        public bool CheckIfPawnAtTheLastRow()
        {
            for (byte i = 0; i < 8; i++)
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
            turnsCount++;
            distanceSum[turn] += (move.dest / 8) - (move.src / 8);
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
            turnsCount--;
            distanceSum[turn] -= (move.dest / 8) - (move.src / 8);
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
