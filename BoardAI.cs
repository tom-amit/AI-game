using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PawnGame.Const;
namespace PawnGame
{
    class BoardAI: Board
    {
        public BoardAI()
        {

        }
        public MorrisNode[] CompPlay()
        {
            Move best = BestMove();
            return new MorrisNode[] { best.src, best.dest, best.ate };
        }
        public int Elapsed(DateTime start) // how much time elapsed on stopper
        {
            DateTime end = DateTime.UtcNow;
            TimeSpan timeDiff = end - start;
            int elapsed = Convert.ToInt32(timeDiff.TotalMilliseconds);
            return elapsed;
        }
        public Move BestMove() // chose best move using alpha-beta for phase 1/2.
        {
            //MAXIMIZIER - PC
            //MINIMIZER - PLAYER
            double value, bestVal, alpha = double.MinValue, beta = double.MaxValue;
            int index, depth, timelimit;
            List<Tuple<Move, double>> listScores = new List<Tuple<Move, double>>();
            List<Move> list = GetAllPossibleMoves(turn);
            DateTime start = DateTime.UtcNow;
            depth = 1;
            timelimit = TIMELIMIT_PLAY;
            do
            {
                listScores.Clear();
                foreach (Move m in list)
                {
                    if (AdvancedPlayHandler(m))
                        listScores.Add(new Tuple<Move, double>(m, (turn == 1) ? WIN_VAL : -WIN_VAL));
                    else
                    {
                        value = AlphaBeta(depth - 1, turn, alpha, beta);
                        MoveBack();
                        listScores.Add(new Tuple<Move, double>(m, value));
                    }
                }
                depth++;
            } while (Elapsed(start) < timelimit / 8);
            listScores.Sort();
            if (turn == 1)
                listScores.Reverse();
            bestVal = listScores[0].Item2;
            index = 1;
            while (index < listScores.Count && listScores[index].Item2 == bestVal)
                index++;
            int r = Const.rnd.Next(index);
            return listScores[r].Item1;
        }
        public double AlphaBeta(int depth, byte playerAB, double alpha, double beta) //Alphabeta
        {
            double value, bestVal;
            List<Move> list = GetAllPossibleMoves(playerAB);
            if (depth == 0)
            {
                if (playerAB == 1)
                {
                    // return evaluation for player 0
                }
                //return evaluation for player 1
            }
            if (playerAB == 1) // maximizer player #1
            {
                bestVal = double.MinValue;
                foreach (Move m in list)
                {
                    if (AdvancedPlayHandler(m))
                        return WIN_VAL;
                    value = AlphaBeta(depth - 1, (byte)(1-playerAB), alpha, beta);
                    MoveBack();
                    bestVal = Math.Max(bestVal, value);
                    alpha = Math.Max(alpha, bestVal);
                    if (beta <= alpha)
                        break;
                }
                return bestVal;
            }
            else // minimizer player #1
            {
                bestVal = double.MaxValue;
                foreach (Move m in list)
                {
                    if (AdvancedPlayHandler(m))
                        return -WIN_VAL;
                    value = AlphaBeta(depth - 1, (byte)(1 - playerAB), alpha, beta);
                    MoveBack();
                    bestVal = Math.Min(bestVal, value);
                    beta = Math.Min(beta, bestVal);
                    if (beta <= alpha)
                        break;
                }
                return bestVal;
            }

        }
        bool AdvancedPlayHandler(Move m) // True if WinVal should be returned in AB.
        {
            if (phase != 0)
            {
                if (m.ate != null && playerPieces[3 - player - 1] - 1 <= 2)
                    return true;
                if (ChoosePiece(m.src))
                    if (PlacePiece(m.src, m.dest) == 1)
                        RemovePiece(m.ate);
            }
            return false;
        }
    }
}
