using System;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace MP4test
{
    public partial class MyMediaPlayer : Form
    {
        private static string path = "";
        public MyMediaPlayer()
        {
            InitializeComponent();
            /*DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
             foreach (FileInfo file in dir.GetFiles("*.mp4"))
            {
                SetPlayerUrl(file.FullName);
                break;
             }*/
        }

        public void SetPlayerUrl(string url)
        {
            if (File.Exists(url))
            {
                path = url;
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = path;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.stop();
            axWindowsMediaPlayer1.Dispose();
            Process.GetCurrentProcess().Kill();
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if ((int)axWindowsMediaPlayer1.playState == 1)
            {
                axWindowsMediaPlayer1.Ctlcontrols.stop();
                axWindowsMediaPlayer1.Dispose();
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
