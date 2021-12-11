using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace soldiers
{
    public partial class GameVisuals : Form
    {
        public GameVisuals()
        {
            InitializeComponent();
            DrawBoard(new Point(100,100), 70);
        }
        private void DrawBoard(Point p, int tileSize)
        {
            Button tile;
            for(int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    tile = new Button()
                    {
                        Size = new Size(tileSize, tileSize),
                        Location = new Point(p.X+tileSize*j, p.Y+ tileSize * i),
                        BackColor = ((i+j)%2==0) ? Color.Black:Color.Gray,
                        TabStop = false,
                        Enabled = false,
                        FlatStyle = FlatStyle.Flat
                    };
                    tile.FlatAppearance.BorderSize = 0;
                    Controls.Add(tile);
                }
            }
        }
    }
}
