using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note_block_bedrock {
    public partial class Form1: Form {
        
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
        }

        public void reDraw() {

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
    }
}
