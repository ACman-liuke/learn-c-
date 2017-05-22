using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace MagicClock
{
    public partial class Form1 : Form
    {
        private static string Account = "";
        private static string Password = "";
        private static string DefaultAccount = "";
        private static string DefaultPassword = "";
        private delegate void ShowTimeContext(string value);
        private ShowTimeContext ShowTimeCallback;
        SynchronizationContext sc;
        public Form1()
        {
            ShowTimeCallback = new ShowTimeContext(ShowNowTime);
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + @"\BackGround.png");
            Thread n = new Thread(NowTime);
            n.Start();

            sc = SynchronizationContext.Current;
        }

        private void ShowNowTime(object time)
        {
            linkLabel1.Text = "Now Time : " + time.ToString();
        }

        private void NowTime()
        {
            while (true)
            {
                string week = "";
                string time = DateTime.Now.ToString();
                string dt = DateTime.Today.DayOfWeek.ToString();
                switch (dt)
                {
                    case "Monday":
                        week = "星期一";
                        break;
                    case "Tuesday":
                        week = "星期二";
                        break;
                    case "Wednesday":
                        week = "星期三";
                        break;
                    case "Thursday":
                        week = "星期四";
                        break;
                    case "Friday":
                        week = "星期五";
                        break;
                    case "Saturday":
                        week = "星期六";
                        break;
                    case "Sunday":
                        week = "星期日";
                        break;
                }
                sc.Post(ShowNowTime, time + " " + week + " " + dt);
                Thread.Sleep(100);
            }
        }

        private void Register()
        {
            if(DefaultAccount != "")
                DefaultAccount = Account;
            if (DefaultPassword != "")
                DefaultPassword = Password;
        }

        private void SetAccount(string account)
        {
            Account = account;
        }

        private void SetPassword(string password)
        {
            Password = password;
        }

        private void ClearDefaultAccountPassword()
        {
            DefaultAccount = "";
            DefaultPassword = "";
        }

        private void ClearAccountPassword()
        {
            Account = "";
            Password = "";
        }

        private string GetAccount()
        {
            return Account;
        }

        private string GetPassword()
        {
            return Password;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearAccountPassword();
            Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void skinWaterTextBox2_TextChanged(object sender, EventArgs e)
        {
            SetPassword(skinWaterTextBox2.Text);
        }

        private void skinWaterTextBox1_TextChanged(object sender, EventArgs e)
        {
            SetAccount(skinWaterTextBox1.Text);
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            ClearDefaultAccountPassword();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //弹出注册框
            Form3 f = new Form3();
            this.Hide();
            f.Show();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            //按下登陆按钮
        }

        private void 闹钟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            this.Hide();
            f.Show();
            f.Text = "闹钟设置";
        }

        private void 自动关机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            this.Hide();
            f.Show();
            f.Text = "定时关机";
        }

        private void 开机自启动设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            this.Hide();
            f.Show();
        }
    }
}
