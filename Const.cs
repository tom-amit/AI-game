using System;
namespace PawnGame
{
    class Const
    {
        public static Random rnd = new Random();
        public const int boardSize = 8;
        public const int TIMELIMIT_PLAY = 4000;
        public const int WIN_VAL = int.MaxValue;
        public const double GAMMA = 0.96;
        public const double TIMER_ERROR = 0.98;
        public const double MATERIAL_WEIGHT = 1;
        public const double DISTANCE_WEIGHT = 0.1;
        public const int CLIENT_MAX_MOVE = 350;

        static public byte ChessNotationToLocation(string location)
        {
            return (byte)(((int.Parse(location[1].ToString()) - 1) * 8) + (location[0] - 'a'));
        }

        static public string LocationToChessNotation(byte location)
        {
            return ((char)('a'+(location%8))).ToString() + ((location/8)+1).ToString();
        }
    }

}
