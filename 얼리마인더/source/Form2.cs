using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace 얼리마인더
{
    public partial class Form2 : Form
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_EXTENDEDKEY = 1;
        private const int KEYEVENTF_KEYUP = 2;
        
        public int Animalcount = 3;
        public string maintext;

        public static void KeyDown(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
        }

        private static void KeyUp(Keys vKey)
        {
            keybd_event((byte)vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        public void Form2_Load(object sender, EventArgs e)
        {
            if (Animalcount == 1)
            {
                this.pictureBox1.Image = Properties.Resources.멍뭉이_3;
            }
            else if (Animalcount == 2)
            {
                this.pictureBox1.Image = Properties.Resources.곰먐미_3;
            }
            else if (Animalcount == 3)
            {
                this.pictureBox1.Image = Properties.Resources.검_3;
            }
            else
            {
                this.pictureBox1.Image = Properties.Resources.터키_3;
            }

            string[] line = maintext.Split(new char[] { ' ' });

            for (int i = 0; i < line.Length; i++)
            {
                this.WriteText.Text += line[i] + Environment.NewLine;
            }

            WriteText.Parent = this;
            timer1.Start();
        }

        public Form2()
        {
            this.Location = new Point(1250, 500);
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            KeyDown(Keys.LWin);
            KeyDown(Keys.D);
            KeyUp(Keys.LWin);
            KeyUp(Keys.D);

            this.Visible = false;
            System.Threading.Thread.Sleep(200);

            KeyDown(Keys.Alt);
            KeyDown(Keys.D1);
            KeyUp(Keys.Alt);
            KeyUp(Keys.D1);
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Location = new Point(this.Location.X - 5, 500);
            if (this.Location.X <= 1080) timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void WriteText_Click(object sender, EventArgs e)
        {

        }
    }
}
