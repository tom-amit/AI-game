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
        Board board;
        bool chosen;
        Button chosenBtn;
        public GameVisuals()
        {
            board = new Board();
            board.SetupBoard();
            InitializeComponent();
            DrawBoard(new Point(100,100), 70);
            chosen = false;
            turnLabel.Text = "Turn: p1";
        }
        private void DrawBoard(Point p, int tileSize)
        {
            BitArray whitePawns = board.GetWhitePawns(), blackPawns = board.GetBlackPawns();

            Button tile;
            for(int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    tile = new Button()
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
                    tile.Click += new EventHandler(ClickHandler);
                    tile.FlatAppearance.BorderSize = 0;
                    Controls.Add(tile);
                }
            }
        }
        private void ClickHandler(object sender, System.EventArgs e)
        {
            Button b = (Button)sender;
            if (!chosen)
            {
                chosen = true;
                chosenBtn = b;
            }
            else
            {
                if(board.Move(Convert.ToByte(chosenBtn.Tag), Convert.ToByte(b.Tag)))
                {
                    b.Text = chosenBtn.Text;
                    turnLabel.Text = "Turn: " + b.Text;
                    chosenBtn.Text = "";
                    chosen = false;
                }
                else
                {
                    chosen = false;
                }
            }
        }
    }
}
