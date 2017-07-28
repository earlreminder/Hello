using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Net;
using System.IO;

namespace 얼리마인더
{
    public partial class Form1 : Form
    {
        private bool bAltAndNum;
        private bool bAltOrNum;
        public string maintext;
        public int Animalcount = 1;
        public bool OnAlarm = true;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        event KeyboardHooker.HookedKeyboardUserEventHandler HookedKeyboardNofity;

        public Point ptRect = new Point(0, 0);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = @"C:\Users\dsm2017\Desktop\earlysave.txt";
            StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
            string str;
            while ((str = sr.ReadLine()) != null)
            {
                this.textBox1.Text += str + Environment.NewLine;
            }
            sr.Close();

            request.Start();
            this.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);

            HookedKeyboardNofity += new KeyboardHooker.HookedKeyboardUserEventHandler(Form1_HookedKeyboardNofity);
            KeyboardHooker.Hook(HookedKeyboardNofity);
        }

        private void Form1_FormClosing(object sender, FormClosedEventArgs e)
        {
            KeyboardHooker.UnHook();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            Cover.Visible = false;
        }

        private void Form1_Deactivated(object sender, EventArgs e)
        {
            Cover.Visible = true;

            this.pictureBox3.Visible = false;
            this.pictureBox5.Visible = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private long Form1_HookedKeyboardNofity(int iKeyWhatHappende, int vkCode)
        {
            long lResult = 0;

            if (vkCode == 49 && iKeyWhatHappende == 32)
            {
                bAltAndNum = true;
                bAltOrNum = false;
                lResult = 0;
                timer1.Interval = 50;
                timer1.Start();
            }
            else if (vkCode == 49 && iKeyWhatHappende == 32)
            {
                bAltAndNum = false;
                bAltOrNum = true;
                lResult = 0;
            }
            else if (vkCode == 49 && iKeyWhatHappende == 32)
            {
                bAltAndNum = false;
                bAltOrNum = true;
                lResult = 0;
            }
            else if (!bAltAndNum && bAltOrNum && (vkCode == 49 || vkCode == 164))
            {
                bAltOrNum = false;
                lResult = 0;
                timer1.Interval = 50;
                timer1.Start();
            }
            else
            {
                bAltAndNum = false;
                bAltOrNum = false;
                lResult = 0;
            }

            return lResult;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopMost = true;
            timer2.Start();
            this.TopMost = false;
            this.Focus();
            timer1.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            OnAlarm = !OnAlarm;

            if (OnAlarm) this.pictureBox3.Image = Properties.Resources.on;
            else this.pictureBox3.Image = Properties.Resources.off;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.pictureBox3.Visible = !this.pictureBox3.Visible;
            this.pictureBox5.Visible = !this.pictureBox5.Visible;
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBox4.BackColor = Color.FromArgb(53, 28, 1);
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            this.pictureBox4.BackColor = Color.FromArgb(71, 43, 5);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Animalcount++;
            Animalcount = Animalcount % 5 + 1;

            if (Animalcount == 1)
            {
                this.pictureBox5.Image = Properties.Resources._1;
            }
            else if (Animalcount == 2)
            {
                this.pictureBox5.Image = Properties.Resources._2;
            }
            else if (Animalcount == 3)
            {
                this.pictureBox5.Image = Properties.Resources._3;
            }
            else
            {
                this.pictureBox5.Image = Properties.Resources._4;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            this.pictureBox3.Visible = false;
            this.pictureBox5.Visible = false;
        }

        private void request_Tick(object sender, EventArgs e)
        {
            WebRequest request = WebRequest.Create("http://cbangserver.netai.net/usrdata/src/code.php");
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            
            if (responseFromServer != null && responseFromServer != maintext)
            {
                maintext = responseFromServer;
                this.textBox1.Text = this.textBox1.Text + Environment.NewLine + Environment.NewLine + maintext;

                if (OnAlarm)
                {
                    Form2 dlg = new Form2();
                    dlg.maintext = this.maintext;
                    dlg.Animalcount = this.Animalcount;
                    dlg.ShowDialog();
                }
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string path = @"C:\Users\dsm2017\Desktop\earlysave.txt";
            StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default); 
            sw.WriteLine(this.textBox1.Text);

            sw.Close();
        }

        private void Cover_Click(object sender, EventArgs e)
        {

        }
    }

    public class KeyboardHooker
    {
        private delegate long HookedKeyboardEventHandler(int nCode, int wParam, IntPtr lParam);
        public delegate long HookedKeyboardUserEventHandler(int iKeyWhatHappened, int vkCode);

        private const int WH_KEYBOARD_LL = 13;
        private static long m_hDllKbdHook;
        private static KBDLLHOOKSTRUCT m_KbDllHs = new KBDLLHOOKSTRUCT();
        private static IntPtr m_LastWindowHWnd;
        public static IntPtr m_CurrentWindowHWnd;
        
        private static HookedKeyboardEventHandler m_LlKbEh = new HookedKeyboardEventHandler(HookedKeyboardProc);
        private static HookedKeyboardUserEventHandler m_fpCallbkProc = null;

        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [DllImport(@"kernel32.dll", CharSet = CharSet.Auto)]
        private static extern void CopyMemory(ref KBDLLHOOKSTRUCT pDest, IntPtr pSource, long cb);
        [DllImport(@"user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();
        [DllImport(@"user32.dll", CharSet = CharSet.Auto)]
        private static extern uint GetAsyncKeyState(int vKey);
        [DllImport(@"user32.dll", CharSet = CharSet.Auto)]
        private static extern long CallNextHookEx(long hHook, long nCode, long wParam, IntPtr lParam);
        [DllImport(@"user32.dll", CharSet = CharSet.Auto)]
        private static extern long SetWindowsHookEx(int idHook, HookedKeyboardEventHandler lpfn, long hmod, int dwThreadId);
        [DllImport(@"user32.dll", CharSet = CharSet.Auto)]
        private static extern long UnhookWindowsHookEx(long hHook);

        private const int HC_ACTION = 0;
        private static long HookedKeyboardProc(int nCode, int wParam, IntPtr lParam)
        {
            long lResult = 0;

            if (nCode == HC_ACTION)
            {
                unsafe
                {
                    CopyMemory(ref m_KbDllHs, lParam, sizeof(KBDLLHOOKSTRUCT));
                }

                m_CurrentWindowHWnd = GetForegroundWindow();

                if (m_CurrentWindowHWnd != m_LastWindowHWnd)
                {
                    m_LastWindowHWnd = m_CurrentWindowHWnd;
                }

                if (m_fpCallbkProc != null)
                {
                    lResult = m_fpCallbkProc(m_KbDllHs.flags, m_KbDllHs.vkCode);
                    
                }
            }
            else if (nCode <0)
            {
                return CallNextHookEx(m_hDllKbdHook, nCode, wParam, lParam);
            }

            return lResult;
        }

        public static bool Hook(HookedKeyboardUserEventHandler callBackEventHandler)
        {
            bool bResult = true;
            m_hDllKbdHook = SetWindowsHookEx(
                (int)WH_KEYBOARD_LL,
                m_LlKbEh,
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(),
                0);

            if (m_hDllKbdHook == 0)
            {
                bResult = false;
            }
            KeyboardHooker.m_fpCallbkProc = callBackEventHandler;

            return bResult;
        }

        public static void UnHook()
        {
            UnhookWindowsHookEx(m_hDllKbdHook);
        }
    }
}
