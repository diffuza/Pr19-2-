using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using static System.Windows.Forms.LinkLabel;


namespace Pr19_2_
{

    public partial class Form1 : Form
    {
        private bool drawing;
        private int nLine, nPoint;
        private int t;
        private const int n20 = 20;
        private Bitmap bitmap;
        private Graphics g0;
        public Form1()
        {
         InitializeComponent();
            InitializeGraphics();
            InitializeTimer();
            InitializeMouseEvents();

            drawing = false;
            nLine = -1;
            nPoint = -1;
            t = 0;
            TLines.Init(5);

        }
        private void InitializeGraphics()
        {
            bitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
            g0 = CreateGraphics();
        }

        private void InitializeTimer()
        {
            timer1 = new Timer();
            timer1.Interval = 100; // Интервал в миллисекундах
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void InitializeMouseEvents()
        {
            MouseDown += Form1_MouseDown;
            MouseMove += Form1_MouseMove;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            drawing = true;
            nPoint = TLines.FindPoint(ref nLine, (float)XX(e.X), (float)YY(e.Y));
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                TLines.lines[nLine][nPoint].X = (float)XX(e.X);
                TLines.lines[nLine][nPoint].Y = (float)YY(e.Y);
                Draw();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            t++;
            timer1.Enabled = t < n20;
            int L = TLines.lines[0].Length;
            float x1, x2, y1, y2;

            for (int i = 0; i < L; i++)
            {
                x1 = TLines.lines[0][i].X;
                y1 = TLines.lines[0][i].Y;
                x2 = TLines.lines[1][i].X;
                y2 = TLines.lines[1][i].Y;
                TLines.tmpLines[i].X = x1 + t * (x2 - x1) / n20;
                TLines.tmpLines[i].Y = y1 + t * (y2 - y1) / n20;
            }
            Draw();
        }
        private void Draw()
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Color cl = Color.FromArgb(255, 255, 255);
                g.Clear(cl);
                int L = 0;
                if (TLines.lines[0] != null)
                    L = TLines.lines[0].Length;

                Point[] p = new Point[L];
                float x, y;

                if (L > 0)
                {
                    if (!timer1.Enabled)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            for (int i = 0; i < L; i++)
                            {
                                x = TLines.lines[k][i].X;
                                y = TLines.lines[k][i].Y;
                                g.DrawRectangle(Pens.Black, II(x) - 2, JJ(y) - 2, 4, 4);
                                p[i].X = II(x);
                                p[i].Y = JJ(y);
                            }
                            g.DrawClosedCurve(Pens.Black, p);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < L; i++)
                        {
                            x = TLines.tmpLines[i].X;
                            y = TLines.tmpLines[i].Y;
                            p[i].X = II(x);
                            p[i].Y = JJ(y);
                        }
                        g.DrawClosedCurve(Pens.Black, p);
                    }
                }
            }
            g0.DrawImage(bitmap, ClientRectangle);
        }

        public static double XX(int x)
        {
            return (x - 200.0) / 200.0;
        }

        public static double YY(int y)
        {
            return (y - 200.0) / -200.0;
        }

        public static int II(float x)
        {
            return (int)(x * 10.0 + 100.0);
        }

        public static int JJ(float y)
        {
            return (int)(y * -10.0 + 200.0);
        }
    }

    public class TLines
    {
        public static PointF[][] lines = new PointF[2][];
        public static PointF[] tmpLines;

        public static void Init(int n)
        {
            float r = 6;
            lines[0] = new PointF[n];
            lines[1] = new PointF[n];
            tmpLines = new PointF[n];
            double x, y;


            for (int i = 0; i < n; i++)
            {
                x = r * Math.Cos(Math.PI * 2 / n * i);
                y = r * Math.Sin(Math.PI * 2 / n * i);
                lines[0][i] = new PointF((float)(-3 + x), (float)y);
                lines[1][i] = new PointF((float)(3 + x), (float)y);
            }
        }

        public static int FindPoint(ref int nLine, float x, float y)
        {
            int result = -1;
            nLine = -1;
            double min = 1000;

            int L = lines[0].Length;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < L; j++)
                {
                    float X = lines[i][j].X;
                    float Y = lines[i][j].Y;
                    double d = Math.Sqrt((X - x) * (X - x) + (Y - y) * (Y - y));

                    if (d < min)
                    {
                        min = d;
                        nLine = i;
                        result = j;
                    }
                }
            }
            return result;
        }
    }
}

    

    

    
    




  


        




