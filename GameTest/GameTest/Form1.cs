using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace GameTest {
    public partial class Form1: Form {

        private System.Windows.Forms.Timer timer;
        private DateTime lastTickTime;

        const double pixelSize = 20;

        Block[] walls = new Block[10];

        private Bitmap canvas;
        Random random = new Random();

        public Form1() {
            InitializeComponent();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += gameTimer_Tick;
            timer.Start();
            lastTickTime = DateTime.Now;

            canvas = new Bitmap(gameCanvas.Width, gameCanvas.Height);
            gameCanvas.BackgroundImage = canvas;

            walls[0] = new Block(0, -3, 10, 1);
            walls[1] = new Block(5, -2, 1, 1);
            walls[2] = new Block(-2, 0, 1, 1);
        }

        private void gameTimer_Tick(object sender, EventArgs e) {

            TimeSpan deltaTime = DateTime.Now - lastTickTime;

            lastTickTime = DateTime.Now;

            player.yVel -= 0.01;
            
            player.onGround = false;

            for (int i = 0; i < walls.Length; i++) {
                if (walls[i] != null) {
                    walls[i].collideFloor();
                }
            }

            if (key.space && player.onGround) {
                player.yVel += 0.3;
            }

            if (key.a) {
                player.xVel -= 0.055;
            }
            if (key.d) {
                player.xVel += 0.055;
            }

            player.xVel = player.xVel * 0.77;

            for (int i = 0; i < walls.Length; i++) {
                if (walls[i] != null) {
                    walls[i].collide();
                }
            }

            player.x += player.xVel;
            player.y += player.yVel;

            using (Graphics g = Graphics.FromImage(canvas)) {
                g.Clear(Color.White);
                g.FillRectangle(brush.red, (int)Math.Round(GtSX(player.x - 0.5)), (int)Math.Round(GtSY(player.y - 0.5)), (int)Math.Round(pixelSize), (int)Math.Round(pixelSize));
                for(int i = 0; i < walls.Length; i++) {
                    if (walls[i] != null) {
                        g.FillRectangle(brush.black, (int)Math.Round(GtSX(walls[i].x - (walls[i].dx / 2))), (int)Math.Round(GtSY(walls[i].y - (walls[i].dy / 2))), (int)Math.Round(walls[i].dx * pixelSize), (int)Math.Round(walls[i].dy * pixelSize));
                    }
                }
            }
            gameCanvas.Invalidate();

        }

        private double GtSX(double x) {
            return (gameCanvas.Width / 2) + (x * pixelSize) - (camera.x * pixelSize);
        }

        private double GtSY(double y) {
            return (gameCanvas.Height / 2) - (y * pixelSize) + (camera.y * pixelSize);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.W) {
                key.w = true;
            }
            if (e.KeyCode == Keys.A) {
                key.a = true;
            }
            if (e.KeyCode == Keys.S) {
                key.s = true;
            }
            if (e.KeyCode == Keys.D) {
                key.d = true;
            }
            if (e.KeyCode == Keys.Space) {
                key.space = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.W) {
                key.w = false;
            }
            if (e.KeyCode == Keys.A) {
                key.a = false;
            }
            if (e.KeyCode == Keys.S) {
                key.s = false;
            }
            if (e.KeyCode == Keys.D) {
                key.d = false;
            }
            if (e.KeyCode == Keys.Space) {
                key.space = false;
            }
        }
    }
}

public class player {
    public static double x = 0;
    public static double y = 0;
    public static double xVel = 0;
    public static double yVel = 0;
    public static bool onGround = false;
}

public class camera {
    public static double x = 0;
    public static double y = 0;
}

public class key {
    public static bool w = false;
    public static bool a = false;
    public static bool s = false;
    public static bool d = false;
    public static bool space = false;
}

public class brush {
    public static SolidBrush red = new SolidBrush(Color.Red);
    public static SolidBrush black = new SolidBrush(Color.Black);
}

public class Block {
    // Properties
    public double x { get; set; }
    public double y { get; set; }
    public double dx { get; set; }
    public double dy { get; set; }

    // Constructor
    public Block(double x, double y, double dx, double dy) {
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
    }

    // Methods
    public void collideFloor() {

        if(player.x + 0.5 > this.x - (this.dx / 2) && player.x - 0.5 < this.x + (this.dx / 2)) {
            if ((player.y - 0.5) >= this.y + (this.dy / 2) && (player.y - 0.5) + player.yVel < this.y + (this.dy / 2)) {
                player.y = this.y + (this.dy / 2) + 0.5;
                player.yVel = 0;
                player.onGround = true;
            }
        }
        
    }
    public void collide() {

        if (player.x + 0.5 > this.x - (this.dx / 2) && player.x - 0.5 < this.x + (this.dx / 2)) {
            if ((player.y + 0.5) <= this.y - (this.dy / 2) && (player.y + 0.5) + player.yVel > this.y - (this.dy / 2)) {
                player.y = this.y - (this.dy / 2) - 0.5;
                player.yVel = 0;
            }
        }

        if (player.y + 0.5 > this.y - (this.dy / 2) && player.y - 0.5 < this.y + (this.dy / 2)) {

            if ((player.x - 0.5) >= this.x + (this.dx / 2) && (player.x - 0.5) + player.xVel < this.x + (this.dx / 2)) {
                player.x = this.x + (this.dx / 2) + 0.5;
                player.xVel = 0;
            }

            if ((player.x + 0.5) <= this.x - (this.dx / 2) && (player.x + 0.5) + player.xVel > this.x - (this.dx / 2)) {
                player.x = this.x - (this.dx / 2) - 0.5;
                player.xVel = 0;
            }

        }

    }
}