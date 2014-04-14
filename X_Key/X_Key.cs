using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
namespace X_Key
{
    public partial class X_Key : Form
    {
        public X_Key()
        {
            InitializeComponent();
        }

        KeyHook hook = new KeyHook();
        private const string config = "config.dat";
        bool isHookEnable = true;
        private const int KEY_QUOTLEFT = 219;//键盘上 [ 键的代码
        private const int KEY_QUOTRIGHT = 221;//键盘上 ] 键的代码

        private const uint WM_SETTEXT = 0x000C;
        private const uint WM_CHAR = 0x0102;
        private const uint EM_SETSEL = 0x00B1;

        private const int WM_CLOSE = 0x10;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;




        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }
        }


        void hook_OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Scroll)
            {
                isHookEnable = !isHookEnable;
                this.Text = isHookEnable ? "X_KEY-开启" : "X_KEY-停用";
                notifyIcon1.Text = this.Text;
            }
            if (isHookEnable)
            {
                IntPtr war3 = FindWindow(null, "Warcraft III");
                if (war3 != IntPtr.Zero)
                {
                    SetForegroundWindow(war3);
                    if (this.ckbowen.Checked)
                        SendMessage(war3, WM_KEYDOWN, KEY_QUOTLEFT, 0);//按下[,可显示友方单位生命值
                    if (this.ckbenemy.Checked)
                        SendMessage(war3, WM_KEYDOWN, KEY_QUOTRIGHT, 0);//按下],可显示友方单位生命值
                    if (hash.ContainsKey(e.KeyValue.ToString()))
                    {
                        SendMessage(war3, WM_KEYDOWN, int.Parse(hash[e.KeyValue.ToString()].ToString()), 0);
                        SendMessage(war3, WM_KEYUP, int.Parse(hash[e.KeyValue.ToString()].ToString()), 0);
                    }
                   // MessageBox.Show(e.KeyValue.ToString());
                }
            }
 
        }
        [DllImport("USER32.DLL")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.DLL")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("USER32.DLL")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        private static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
        }
        Hashtable hash = new Hashtable();
        private void btnsave_Click(object sender, EventArgs e)
        {
            hash.Clear();
            try
            {
                if (txtnum1.Text != "" && txtnum1.Text != null)
                {
                    hash.Add(Asc(txtnum1.Text).ToString(), "97");
                }
                if (txtnum2.Text != "" && txtnum2.Text != null)
                {
                    hash.Add(Asc(txtnum2.Text).ToString(), "98");
                }
                if (txtnum4.Text != "" && txtnum4.Text != null)
                {
                    hash.Add(Asc(txtnum4.Text).ToString(), "100");
                }
                if (txtnum5.Text != "" && txtnum5.Text != null)
                {
                    hash.Add(Asc(txtnum5.Text).ToString(), "101");
                }
                if (txtnum7.Text != "" && txtnum7.Text != null)
                {
                    hash.Add(Asc(txtnum7.Text).ToString(), "103");
                }
                if (txtnum8.Text != "" && txtnum8.Text != null)
                {
                    hash.Add(Asc(txtnum8.Text).ToString(), "104");
                }
                //if (hash.Count > 0)
                //{
                //    try
                //    {
                //        FileStream fs = new FileStream(config, FileMode.OpenOrCreate);
                //        BinaryFormatter bf = new BinaryFormatter();
                //        bf.Serialize(fs, hash);
                //        fs.Close();
                //    }
                //    catch
                //    {
                //        MessageBox.Show("保存设置失败!", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //}
                this.Hide();
            }
            catch (Exception)
            {
                MessageBox.Show( "按键重复，请核对！","肿么出错了！",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
          

        }

        //关闭或最小化时隐藏
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;
            const int SC_MINIMIZE = 0xF020;
            if (m.Msg == WM_SYSCOMMAND && ((int)m.WParam == SC_MINIMIZE || (int)m.WParam == SC_CLOSE))
            {
                //最小化到系统栏 
                this.Hide();
                return;
            }
            base.WndProc(ref m);
        }

        private void X_Key_Load(object sender, EventArgs e)
        {
            hook.OnKeyDownEvent += new KeyEventHandler(hook_OnKeyDownEvent);
        }

        private void Uiexit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showUI_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
    }
}
