using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PawnGame
{
    public partial class GameVisuals : Form
    {
        BoardAI board;
        bool chosen, isAI;
        Button chosenBtn;
        //Button prevPawn;
        Button[] tiles;
        public GameVisuals()
        {
            tiles = new Button[64];
            board = new BoardAI();
            board.SetupBoard();
            InitializeComponent();
            DrawBoard(new Point(100,100), 70);
            chosen = false;
            isAI = false;
            turnLabel.Text = "Turn: p1";
        }
        private void DrawBoard(Point p, int tileSize)
        {
            BitArray whitePawns = board.GetWhitePawns(), blackPawns = board.GetBlackPawns();

            for(int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    tiles[i * 8 + j] = new Button()
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(p.X + tileSize * j, p.Y + tileSize * i),
                        BackColor = ((i + j) % 2 == 0) ? Color.Black : Color.Gray,
                        TabStop = false,
                        Text = whitePawns[i * 8 + j] ? "p1" : (blackPawns[i * 8 + j] ? "p2" : ""),
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
            for(int i = 0; i < 64; i++)
            {
                if (board.GetWhitePawns()[i]) 
                    tiles[i].Text = "p1";
                else if (board.GetBlackPawns()[i])
                    tiles[i].Text = "p2";
                else
                    tiles[i].Text = "";
            }
        }
        private void ClickHandler(object sender, System.EventArgs e)
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
                if(board.Move(Convert.ToByte(chosenBtn.Tag), Convert.ToByte(b.Tag)))
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
                        if (isAI)
                        {
                            CompPlay();
                        }
                    }
                }
                else
                {
                    chosen = false;
                }
                chosenBtn.FlatAppearance.BorderSize = 0;
            }
        }

        private void CompPlay()
        {
            if (turnLabel.Text == "p2")
            {
                board.CompPlay();
                turnLabel.Text = "Turn: p" + (board.turn + 1).ToString();
                UpdateBoardVisuals();
                if (board.CheckIfMatchEnd())
                {
                    MessageBox.Show("PLAYER " + (board.turn + 1).ToString() + " LOSES");
                }
            }
        }

        private void UndoMoveVisual(object sender, EventArgs e)
        {
            board.UnmakeMove();
            UpdateBoardVisuals();
        }

        private void isAICheck_CheckedChanged(object sender, EventArgs e)
        {
            isAI = ((CheckBox)sender).Checked;
            if(isAI && chosen)
            {
                chosen = false;
                chosenBtn.FlatAppearance.BorderSize = 0;
            }
        }
    }
}
