using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace FBForReal
{
    internal class Game
    {
        //Dont translate This are helpers

        Form parent;

        public Brush bbrush = new SolidBrush(Color.Yellow);

        public Brush pbrush = new SolidBrush(Color.Green);

        public Brush brush = new SolidBrush(Color.Magenta);
        public Font font = new Font("Arial", 12);

        public Bitmap bmp = new Bitmap(800, 600);

        public Brush dbrush = new SolidBrush(Color.Black);

        //end of helpers

        //bird

        public const int bx = 100;
        public int by = 300;
        public int byp = 300;

        public const int ba = 3;
        public int bv = MIN_SPEED;

        public const int bheight = 60;
        public const int bwidth = 100;

        public const int MIN_SPEED = -30;
        public const int MAX_SPEED = 20;


        //end of bird

        //pipes

        public const int arrSize = 50;

        public int[] px = new int[arrSize];
        public int[] py = new int[arrSize];

        public const int pwidth = 100;
        public const int pheight = 300;

        public const int pv = 10;

        public int head = 0;
        public int tail = 0;
        public int cap = 0;

        public const int createTime = 50;
        public int currTime = 5;

        Random rand = new Random();

        //end of pipes

        //screen

        public const int screenHeight = 600;
        public const int screenWidht = 800;

        //end of screen

        //space detection

        public bool spaceC = false;
        public bool spaceP = false;

        //end of space detection

        //control bools

        public bool playing = false;
        public bool startGame = true;

        // end of control bools

        //score DONT TRANSLATE

        public int score = 0;
        public bool pscore = false;
        public bool cscore = false;

        //end of score

        public Game(Form parent)
        {
            
            this.parent = parent;

            using (Graphics g = Graphics.FromImage(bmp))    //Dont translate
            {

                g.FillRectangle(dbrush, 0, 0, 800, 600);

            }


        }

        public void gameLoop()
        {

            if (startGame)
            {
                playing = true;     //dont change the order
                startGame = false;
                restart();
            }

            if (playing == false)
                return;

            update();

            colision();

            parent.Refresh();

        }

        public void update()
        {
            if (spaceC && !spaceP) // space == true u prevodjenju
                bv = MIN_SPEED;

            //moze da pojede input ako se desi prekid u ovom trenutku

            spaceP = spaceC; // space = false u prevodjenju

            spaceC = false;

            //bird update

            //potrebne promenljive : space, MAX_SPEED, by, bv, ba, bheight, screenHeight; Total = 7

            byp = by;

            bv += ba;

            if (bv > MAX_SPEED)
                bv = MAX_SPEED;

            by += bv;

            if (by <= 0)
            {
                bv = 0;
                by = 0;
            }

            if (by + bheight >= screenHeight)
            {
                bv = 0;
                by = screenHeight - bheight;
            }

            //pipes update

            //pipes creation

            //potrebne promenljive : currTime, x, y, head, arrSize, cap, createTime, screenHeight, pheight; Total = 9

            currTime--;

            if (currTime <= 0)
            {
                px[head] = screenWidht;
                py[head] = rand.Next() % (screenHeight - pheight); // figure out random

                head = head + 1;

                if (head >= arrSize)
                    head = head - arrSize;

                cap = cap + 1;

                currTime = createTime;

            }

            //pipes move

            //-------------------------------------------------------------------------------------------------------------------

            /*ubrzanje1 ->
            if (px[tial] + pwidth <=0)
            {
                tail++;
                cap--;
                if (tail > arrSize)
                    tail = 0;
            }
             */


            //-------------------------------------------------------------------------------------------------------------------

            /* uprzanje2 -> 
            for (int* t = x[tail]; t != x[head]; t++) //ima problem ako je buffer pun, ali u praksi ne bi trebalo da se desava
            {

            if (t > x[arrSize]) 
                t = x; 

            *t -= pv; 
             
            if (*t + pwidth <= 0)
                del++;

            }*/

            //-------------------------------------------------------------------------------------------------------------------

            //potrebne promenljive : x, tail, arrSize, cap, pv, pwidth; del, i, t; Total = 9

            
            int del = 0;

            for (int i = 0; i < cap; i++) 
            {

                int t = tail + i;

                if (t >= arrSize)
                    t = t - arrSize;

                px[t] -= pv;

                if (px[t] + pwidth + pv <= 0)
                    del++;

            }

            if (del > 0)
            {
                cap -= del;
                tail += del;
                if (tail >= arrSize)
                    tail = tail - arrSize;

            }
            

        }

        public void colision()
        {

            //ubrzanje -> check only head or head + 1
            //isto ubrzanje kao za pipe upsate

            //potrebne promenljive : x, y, tail, arrSize, cap, pwidth, pheaight, bx, xy, bwidth, bheight; i, t; Total = 13

            //cscore = false;

            for (int i = 0; i < cap; i++)
            {

                int t = tail + i;                                       //% -> bird, | -> pipe
                if (t >= arrSize)
                    t = t - arrSize;
                if (px[t] < bx + bwidth && px[t] + pwidth > bx)         //true if:  %   |   %   |   or  |   %   |   % 
                {

                    //cscore = true;

                    if (by > py[t] && by + bheight < py[t] + pheight)   //true if:  |   %   %   |
                        continue;

                    playing = false;

                    //restart();//ne mora da se pravi funkcija

                    break; //opciono

                }

            }

            /*
            if (cscore == false && pscore == true)
                score++;

            pscore = cscore;
            */

        }

        public void restart()
        {

            using (Graphics g = Graphics.FromImage(bmp))
            {

                for (int i = 0; i < cap; i++)
                {

                    int t = tail + i;
                    if (t >= arrSize)
                        t = t - arrSize;

                    g.FillRectangle(dbrush, px[t], 0, pwidth, py[t]);
                    g.FillRectangle(dbrush, px[t], py[t] + pheight, pwidth, screenHeight - py[t] - pheight);


                }

                g.FillRectangle(dbrush, bx, by, bwidth, bheight);

            }

            

            //bird
            by = 300;
            bv = MIN_SPEED;

            //pipes
            head = 0;
            tail = 0;
            cap = 0;

            currTime = 5;

            //space = false
            spaceP = false;

            //score DONT TRANSLATE

            score = 0;
            pscore = false;
            cscore = false;

            /*
            using (Graphics g = Graphics.FromImage(bmp))
            {

                g.FillRectangle(dbrush, 0, 0, 800, 600);
                g.FillRectangle(bbrush, bx, by, bwidth, bheight);

            }
            */

        }

        public void kyeDetected(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Space)
            {
                spaceC = true;
            }

            if (e.KeyCode == Keys.Space && playing == false && startGame == false) // moze bez starGame ali je onda redosled bitan u gameLoop-u
                startGame = true;

        }

        public Bitmap draw(Graphics g)
        {

            //bmp = new Bitmap(800, 600);

            using (g = Graphics.FromImage(bmp))
            {

                g.FillRectangle(dbrush, bx, byp, bwidth, bheight);//if bv > 0 then {...} else {...}

                for (int i = 0; i < cap; i++)
                {

                    int t = tail + i;
                    if (t > arrSize)
                        t = t - arrSize;

                    /*
                    g.FillRectangle(pbrush, px[t], 0, pwidth, py[t]);
                    g.FillRectangle(pbrush, px[t], py[t] + pheight, pwidth, screenHeight - py[t] - pheight);
                    */


                    g.FillRectangle(pbrush, px[t]         , 0, pv, py[t]);
                    g.FillRectangle(dbrush, px[t] + pwidth, 0, pv, py[t]);

                    g.FillRectangle(pbrush, px[t]         , py[t] + pheight, pv, screenHeight - py[t] - pheight);
                    g.FillRectangle(dbrush, px[t] + pwidth, py[t] + pheight, pv, screenHeight - py[t] - pheight);
                    
                }

                g.FillRectangle(bbrush, bx, by, bwidth, bheight);

                //g.FillRectangle(dbrush, 750, 0, 60, 40);

                //g.DrawString(score.ToString(), font, brush, new PointF(750, 0));

            }



            return bmp;

        }

    }
}
