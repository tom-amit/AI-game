using System;
using System.Collections.Generic;
using static PawnGame.Const;
namespace PawnGame
{
    class BoardAI : Board
    {
        static DateTime start;
        public BoardAI()
        {

        }
        public void CompPlay()
        {
            Move(BestMove());
        }
        public double Evaluate()
        {
            return count[1] - count[0];// rnd.Next(1,100);
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
            List<Tuple<Move, double>> listScores = new List<Tuple<Move, double>>(), prevIterationList;
            List<Move> list = GetAllPossibleMoves(turn);
            start = DateTime.UtcNow;
            depth = 1;
            do
            {
                prevIterationList = listScores;
                listScores.Clear();
                foreach (Move m in list)
                {
                    if (Elapsed(start) >= TIMELIMIT_PLAY * TIMER_ERROR)
                    {
                        listScores = prevIterationList;
                        break;
                    }
                    if (AdvancedPlayHandler(m))
                        listScores.Add(new Tuple<Move, double>(m, (turn == 1) ? WIN_VAL : -WIN_VAL));
                    else
                    {
                        value = AlphaBeta(depth - 1, turn, alpha, beta);
                        UnmakeMove();
                        listScores.Add(new Tuple<Move, double>(m, value));
                    }
                }
                depth++;
            } while (Elapsed(start) < TIMELIMIT_PLAY * TIMER_ERROR);
            listScores.Sort((num1, num2) => num1.Item2.CompareTo(num2.Item2));
            if (turn == 1)
                listScores.Reverse();
            bestVal = listScores[0].Item2;
            index = 1;
            while (index < listScores.Count && listScores[index].Item2 == bestVal)
                index++;
            Console.WriteLine("Depth reached: {0}", depth);
            Console.WriteLine("Time For search: {0}", (double)Elapsed(start)/1000);
            return listScores[Const.rnd.Next(index)].Item1;
        }
        public double AlphaBeta(int depth, byte playerAB, double alpha, double beta) //Alphabeta
        {
            double value, bestVal;
            List<Move> list = GetAllPossibleMoves(playerAB);
            if (Elapsed(start) >= TIMELIMIT_PLAY * TIMER_ERROR)
            {
                return (playerAB == 1) ? alpha : beta;
            }
            if (depth == 0)
            {
                return Evaluate();
            }
            if (playerAB == 1) // maximizer player #1
            {
                bestVal = double.MinValue;
                foreach (Move m in list)
                {
                    if (AdvancedPlayHandler(m))
                        return WIN_VAL;
                    value = GAMMA * AlphaBeta(depth - 1, (byte)(1 - playerAB), alpha, beta);
                    UnmakeMove();
                    bestVal = Math.Max(bestVal, value);
                    alpha = Math.Max(alpha, bestVal);
                    if (beta <= alpha)
                        break;
                }
                return bestVal;
            }
            else // minimizer player #0
            {
                bestVal = double.MaxValue;
                foreach (Move m in list)
                {
                    if (AdvancedPlayHandler(m))
                        return -WIN_VAL;
                    value = GAMMA * AlphaBeta(depth - 1, (byte)(1 - playerAB), alpha, beta);
                    UnmakeMove();
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
            Move(m);
            if (CheckIfMatchEnd())
            {
                UnmakeMove();
                return true;
            }
            return false;
        }
    }
}
