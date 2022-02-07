using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static PawnGame.Const;

namespace PawnGame
{
    class Client
    {
        public IPAddress ip;
        public int port;

        private Socket sender;
        private byte[] response = new byte[1500];
        int resSize;
        private char[] sendHelp = new char[1500];

        private BoardAI board;
        

        public Client(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.board = new BoardAI();
        }

        public bool Connect()
        {
            sender = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.SendBufferSize = 1500;
            IPEndPoint remoteEP = new IPEndPoint(ip, port);

            try
            {
                Console.WriteLine("Attempting to connect to server");
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        private string RecieveResponse()
        {
            resSize = sender.Receive(response);
            Console.WriteLine("recieved |{0}", Encoding.ASCII.GetString(response, 0, resSize));
            return Encoding.ASCII.GetString(response, 0, resSize);
        }

        private void SendResponse(string data)
        {
            sender.Send(Encoding.ASCII.GetBytes(data));
            Console.WriteLine("sent |{0}", data);
        }

        public void PlayMatch()
        {
            string setup, time, res;
            Move best;
            int myColor = 0;

            if (!Connect())
                return;
            SendResponse("OK");

            setup = RecieveResponse();
            board.SetupBoard(setup);
            SendResponse("OK");


            time = RecieveResponse();
            board.timelimit = ((int.Parse(time.Substring(5)) * 60 * 1000) / CLIENT_MAX_MOVE);
            SendResponse("OK");

            res = RecieveResponse();
            if(res != "Begin")
            {
                board.Move(board.CheckMove(ChessNotationToLocation(res.Substring(0, 2)), ChessNotationToLocation(res.Substring(2, 2)), board.turn));
                myColor = 1;
            }

            
            while (res != "exit" && !board.CheckIfMatchEnd()){
                if (board.turn == myColor)
                {
                    best = board.BestMove();
                    SendResponse(best.GetChessNotation());
                    board.Move(best);
                }
                else
                {
                    res = RecieveResponse();
                    board.Move(board.CheckMove(ChessNotationToLocation(res.Substring(0, 2)), ChessNotationToLocation(res.Substring(2, 2)), board.turn));
                }
            }

            if (res != "exit")
                SendResponse("exit");

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
