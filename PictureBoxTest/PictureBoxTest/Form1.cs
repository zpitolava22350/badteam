using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureBoxTest {

    public partial class Form1: Form {

        private Pen drawingPen = new Pen(Color.Black, 1);
        SolidBrush shadowBrush = new SolidBrush(Color.Black);
        private Bitmap canvas;
        Random random = new Random();

        const int pixelSize = 1;

        int mouseX = 0;
        int mouseY = 0;

        int pixelColor;

        FastNoiseLite noise = new FastNoiseLite();

        private bool isAKeyDown = false;
        private bool isDKeyDown = false;

        //Person wall = new Person(400,300,600,100);

        int counter = 0;
        //private Bitmap clearedCanvas;

        public Form1() {

            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2S);
            InitializeComponent();
            //clearedCanvas = new Bitmap(canvasPanel.Width, canvasPanel.Height);
            canvas = new Bitmap(canvasPanel.Width, canvasPanel.Height);
            canvasPanel.BackgroundImage = canvas;

        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void gameRefresh_Tick(object sender, EventArgs e) {

            /*
            SharedVariablesClass.playerYVel += 0.1;

            if (isAKeyDown) {
                SharedVariablesClass.playerXVel -= 0.1;
            }
            if (isDKeyDown) {
                SharedVariablesClass.playerXVel += 0.1;
            }

            wall.SayHello();

            SharedVariablesClass.playerX += SharedVariablesClass.playerXVel;
            SharedVariablesClass.playerY += SharedVariablesClass.playerYVel;
            */

            using (Graphics g = Graphics.FromImage(canvas)) {
                g.Clear(Color.White);

                pixelColor = 0;

                for (int x = 0; x < canvas.Width; x+=pixelSize) {
                    for(int y = 0; y < canvas.Height; y+=pixelSize) {
                        pixelColor = (int)Math.Floor(Math.Abs(noise.GetNoise(x-mouseX, y-mouseY)) * 250);
                        g.FillRectangle(new SolidBrush(Color.FromArgb(pixelColor, pixelColor, pixelColor)), x, y, pixelSize, pixelSize); 
                    }
                }

                /*
                g.Clear(Color.White);
                g.DrawRectangle(drawingPen, (int)SharedVariablesClass.playerX - 10, (int)SharedVariablesClass.playerY - 10, 20, 20);
                g.DrawRectangle(drawingPen, (int)(wall.x-(wall.dx/2)), (int)(wall.y - (wall.dy / 2)), (int)wall.dx, (int)wall.dy);
                */
            }
            canvasPanel.Invalidate();
            Console.WriteLine($"{++counter}");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.A) {
                isAKeyDown = true;
            } else if (e.KeyCode == Keys.D) {
                isDKeyDown = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.A) {
                isAKeyDown = false;
            } else if (e.KeyCode == Keys.D) {
                isDKeyDown = false;
            }
        }
    }

    public class SharedVariablesClass {
        public static double playerX = 400;
        public static double playerY = 200;
        public static double playerXVel = 0;
        public static double playerYVel = 0;
    }

    /*
    public class Person {
        // Properties
        public double x { get; set; }
        public double y { get; set; }
        public double dx { get; set; }
        public double dy { get; set; }

        // Constructor
        public Person(double x, double y, double dx, double dy) {
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
        }

        // Methods
        public void SayHello() {
            if ((SharedVariablesClass.playerY + 10) <= this.y - (this.dy / 2) &&
                (SharedVariablesClass.playerY + 10) + SharedVariablesClass.playerYVel > this.y - (this.dy / 2)) {
                SharedVariablesClass.playerY = this.y - (this.dy / 2) - 10;
                SharedVariablesClass.playerYVel = 0;
            }
        }
    }
    */

}
