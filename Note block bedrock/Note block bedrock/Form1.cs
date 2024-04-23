using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note_block_bedrock {
    public partial class Form1: Form {

        List<Dictionary<int, Note>> notes = new List<Dictionary<int, Note>>();

        List<int> maxIndex = new List<int>();

        public struct Note {
            public ins instrument;
            public int volume;
            public int key;
            public Note(ins tempIns, int tempKey) {
                instrument = tempIns;
                volume = 100;
                key = tempKey;
            }
        }

        Pen penLightGray = new Pen(Color.FromArgb(200, 200, 200));
        Pen penGray1 = new Pen(Color.FromArgb(100, 100, 100));
        Pen penGray2 = new Pen(Color.FromArgb(70, 70, 70));
        SolidBrush brushLightGray = new SolidBrush(Color.FromArgb(200, 200, 200));
        SolidBrush brushGray = new SolidBrush(Color.FromArgb(60, 60, 60));

        StringFormat centered = new StringFormat();

        public enum ins {
            harp = 0,
            dbass = 1,
            bdrum = 2,
            snare = 3,
            click = 4,
            guitar = 5,
            flute = 6,
            bell = 7,
            chime = 8,
            xyl = 9,
            ironxyl = 10,
            cowbell = 11,
            didgeridoo = 12,
            bit = 13,
            banjo = 14,
            pling = 15
        }

        ins instrumentSelected = ins.harp;

        int displayWidth = 16;
        int displayHeight = 7;

        public Form1() {
            InitializeComponent();
            centered.Alignment = StringAlignment.Center;
            centered.LineAlignment = StringAlignment.Center;
        }

        private void Form1_Resize(object sender, EventArgs e) {
            displayWidth = (this.Width - 786 + 512) / 32;
            displayHeight = (this.Height - 344 + 224) / 32;
            hScrollBar.Location = new Point(235, 61 + (displayHeight * 32));
            hScrollBar.Size = new Size(displayWidth * 32, hScrollBar.Size.Height);
            vScrollBar.Location = new Point(238 + (displayWidth * 32), 58);
            vScrollBar.Size = new Size(vScrollBar.Size.Width, displayHeight * 32);
            picBox.Width = displayWidth*32;
            picBox.Height = displayHeight*32;
        }

        private void volumeBar_Scroll(object sender, EventArgs e) {
            lblVolume.Text = volumeBar.Value.ToString() + "%";
        }

        private void btnPlay_Click(object sender, EventArgs e) {

        }

        private void visualUpdate_Tick(object sender, EventArgs e) {
            picBox.Invalidate();
        }

        private void picBox_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;

            g.FillRectangle(brushGray, 0, 0, picBox.Width, 32);

            g.DrawLine(penLightGray, 0, 32, picBox.Width, 32);
            g.DrawLine(penLightGray, 0, 16, picBox.Width, 16);
            
            for(int i = 0; i < displayWidth; i++) {
                if(i % 4 == 0) {
                    g.DrawLine(penGray1, i * 32, 32, i * 32, picBox.Height);
                    g.DrawString(i.ToString(), DefaultFont, brushLightGray, i * 32, 24, centered);
                } else {
                    g.DrawLine(penGray2, i * 32, 32, i * 32, picBox.Height);
                }
            }

        }

        private void picBox_MouseClick(object sender, MouseEventArgs e) {
            int mouseX = e.X/32;
            int mouseY = e.Y/32;
        }
    }
}
