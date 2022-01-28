using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace PawnGame
{
    public partial class GameVisuals : Form
    {
        BoardAI board;
        bool chosen, isAI1, isAI2;
        Button chosenBtn;

        bool inPlacement;
        byte placementChoice;

        Button[] tiles;
        public GameVisuals()
        {
            inPlacement = true;
            tiles = new Button[64];
            board = new BoardAI();
            InitializeComponent();
            DrawBoard(new Point(590, 590), 70);
            chosen = false;
            isAI1 = false;
            isAI2 = false;
            turnLabel.Text = "Turn: p1";
        }
        private void DrawBoard(Point p, int tileSize)
        {
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    tiles[i * 8 + j] = new Button()
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(p.X - tileSize * j, p.Y - tileSize * i),
                        BackColor = ((i + j) % 2 == 0) ? Color.Black : Color.Gray,
                        TabStop = false,
                        Tag = i * 8 + j,
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,

                    };
                    tiles[i * 8 + j].Click += new EventHandler(ClickHandler);
                    tiles[i * 8 + j].FlatAppearance.BorderSize = 0;
                    Controls.Add(tiles[i * 8 + j]);
                }
            }
        }
        private void UpdateBoardVisuals()
        {
            BitArray whitePawns = board.GetWhitePawns(), blackPawns = board.GetBlackPawns();
            for (int i = 0; i < 64; i++)
            {
                if (whitePawns[i])
                    tiles[i].Text = "p1";
                else if (blackPawns[i])
                    tiles[i].Text = "p2";
                else
                    tiles[i].Text = "";
            }
        }
        private void ClickHandler(object sender, System.EventArgs e)
        {
            if (inPlacement)
            {
                board.SetupAddPiece(Convert.ToByte(((Button)sender).Tag), placementChoice);
                UpdateBoardVisuals();
            }
            else
            {
                Button b = (Button)sender;
                if (!chosen)
                {
                    chosen = true;
                    chosenBtn = b;
                    chosenBtn.FlatAppearance.BorderSize = 2;
                }
                else
                {
                    if (board.Move(Convert.ToByte(chosenBtn.Tag), Convert.ToByte(b.Tag)))
                    {
                        turnLabel.Text = "Turn: p" + (board.turn + 1).ToString();
                        chosen = false;
                        UpdateBoardVisuals();

                        if (board.CheckIfMatchEnd())
                        {
                            MessageBox.Show("PLAYER " + (board.turn + 1).ToString() + " LOSES");
                        }
                        else
                        {
                            CompPlay();
                        }
                    }
                    else
                    {
                        chosen = false;
                    }
                    chosenBtn.FlatAppearance.BorderSize = 0;
                }
            }
        }
        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        } //Wait function 
        private void CompPlay()
        {
            wait(50);
            if (!inPlacement && ((isAI1 && board.turn == 0) || (isAI2 && board.turn == 1)))
            {
                board.CompPlay();
                turnLabel.Text = "Turn: Player" + (board.turn + 1).ToString();
                UpdateBoardVisuals();
                if (board.CheckIfMatchEnd())
                {
                    MessageBox.Show("PLAYER " + (board.turn + 1).ToString() + " LOSES");
                    return;
                }
                CompPlay();
            }
        }

        private void UndoMoveVisual(object sender, EventArgs e)
        {
            board.UnmakeMove();
            UpdateBoardVisuals();
        }

        private void disablePlacement()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            inPlacement = false;
        }

        private void button1_Click(object sender, EventArgs e) //standard placement
        {
            board.SetupBoard();
            UpdateBoardVisuals();
            disablePlacement();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            placementChoice = 0;
            label2.Text = "Currently placing white pawns";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            placementChoice = 1;
            label2.Text = "Currently placing black pawns";
        }
        private void button4_Click(object sender, EventArgs e)
        {
            UpdateBoardVisuals();
            disablePlacement();
        }

        private void isAICheck_CheckedChanged(object sender, EventArgs e)
        {
            isAI1 = ((CheckBox)sender).Checked;
            if (isAI1 && chosen && board.turn == 0)
            {
                chosen = false;
                chosenBtn.FlatAppearance.BorderSize = 0;
            }
            CompPlay();
        }
        private void isAI2Check_CheckedChanged(object sender, EventArgs e)
        {
            isAI2 = ((CheckBox)sender).Checked;
            if (isAI2 && chosen && board.turn == 1)
            {
                chosen = false;
                chosenBtn.FlatAppearance.BorderSize = 0;
            }
            CompPlay();
        }
    }
}
