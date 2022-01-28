using System;
namespace PawnGame
{
    class Const
    {
        public static Random rnd = new Random();
        public const int boardSize = 8;
        public const int TIMELIMIT_PLAY = 6000;
        public const int WIN_VAL = int.MaxValue;
        public const double GAMMA = 0.96;
    }
}
