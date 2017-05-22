using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace JsonFileExtraction
{
    public partial class JsonFileExtraction : Form
    {
        private static string gameinfo = Environment.CurrentDirectory + @"\gameinfo.json";
        public JsonFileExtraction()
        {
            InitializeComponent();
            GetGameListInfo();
        }

        private void GetGameListInfo()
        {
            try
            {
                List<string> listArr = new List<string>();
                DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory); 
                foreach (DirectoryInfo files in dir.GetDirectories())
                {
                    foreach (FileInfo f in files.GetFiles("gameinfo.json"))
                    {
                        string content = File.ReadAllText(f.FullName, Encoding.UTF8);
                        if (content.Contains("gameid") && content.Contains("name") && content.Contains("filename") && content.Contains("progress"))
                        {
                            listArr.Add(content);
                        }
                        else
                        {
                            //Console.WriteLine("获取游戏信息失败，因为没有游戏，或者游戏信息不完整......");
                            label1.Text = ("read game config information fail, maybe information inperfect......");
                        }
                    }
                }

                File.Delete(gameinfo);
                File.AppendAllText(gameinfo, "[", Encoding.UTF8);
                foreach (string info in listArr)
                {
                    int index = listArr.IndexOf(info); //当listArr里存在两个完全相同的值，就不能正确获取值在listArr中的索引
                    File.AppendAllText(gameinfo, info, Encoding.UTF8);
                    if (index != listArr.ToArray().Length - 1)
                    {
                        File.AppendAllText(gameinfo, ",", Encoding.UTF8);
                    }
                }
                File.AppendAllText(gameinfo, "]", Encoding.UTF8);
                label1.Text = "read game config information success，altogether ===》" + listArr.ToArray().Length;
                label1.Text = "The file -> " + gameinfo;
                Thread.Sleep(50);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void JsonFileExtraction_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
