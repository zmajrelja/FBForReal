using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBForReal
{
    public partial class Form1 : Form
    {

        Game game;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(800, 640);
            DoubleBuffered = true;
            game = new Game(this);
            timer1.Interval = 16;
            timer1.Enabled = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            game.kyeDetected(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            game.gameLoop();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Bitmap t = game.draw(e.Graphics);

            e.Graphics.DrawImage(t, 0, 0, 800, 600);

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Space)
            {
                game.spaceC = false;
            }
            */
        }
    }
}
