using System;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Net;
using System.IO.Compression;
using GmMessageApi;

namespace test
{
    class Program
    {
        private static string mac = "";
        private static string host = "127.0.0.1";
        private static string port = "8888";
        private static string head = "http://";
        private static int state = 0;
        private static string filename = "";

        static void Main(string[] args)
        {
            Program p = new Program();
            int i = 0;
            MessageApi m = new MessageApi();
            MessageApi.KeyValueDelegate d = new MessageApi.KeyValueDelegate(p.GetRankValue);
            m.SetRankMethod = p.GetRankValue;
            m.Init(args);
            Console.WriteLine("嘉年华游戏初始化成功后，获取到启动参数个数 ==》{0}", args.Length);
            if (args.Length >= 1 && args[0] != null)
            {
                Console.WriteLine("启动参数一：{0}", args[0]);
            }
            if (args.Length >= 2 && args[1] != null)
            {
                Console.WriteLine("启动参数二：{0}", args[1]);
            }
            if (args.Length >= 3 && args[2] != null)
            {
                Console.WriteLine("启动参数三：{0}", args[2]);
            }
            if (args.Length >= 4 && args[3] != null)
            {
                Console.WriteLine("启动参数四：{0}", args[3]);
            }
            if (args.Length >= 5 && args[4] != null)
            {
                Console.WriteLine("启动参数五：{0}", args[4]);
            }

            m.GameStart();
            Console.WriteLine("嘉年华游戏初始化完成，调用游戏开始API");
            Thread.Sleep(5000);

            Thread.Sleep(2000);
            int all = 0;
            for (i = 0; i < 5; i++)
            {
                m.GameIntegrateSend(1000);
                all = all + 1000;
                Thread.Sleep(500);
            }
            Console.WriteLine("嘉年华游戏同步积分完成，共同步积分==> {0}", all);
            
            for (i = 0; i < 5; i++)
            {
                m.EventRcord("hello world!");
            }
            Console.WriteLine("嘉年华游戏记录游戏时间结束！");
            Thread.Sleep(1000);
            
            m.GameResultSend("100", "2");
            Console.WriteLine("嘉年华游戏发送游戏结果结束！");
            Thread.Sleep(1000);
            
            string res = m.GameStop();
            if (res == null)
                res = "no";
            Console.WriteLine("嘉年华游戏结束！即将退出游戏{0}", res);
            Thread.Sleep(1000);
            Console.WriteLine("嘉年华游戏结束！");
            Console.ReadKey();
        }

        private void GetRankValue(string key, string value)
        {
            Console.WriteLine("管理员修改倍率为 ==》 key = {0}, value = {1}", key, value);
        }

        //初始化函数，从文件获取中控的host和port配置
        private void Init()
        {
            string content = "";
            string path = "..\\daemon\\device\\cfgmgr";
            try
            {
                content = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                throw e;
            }

            JObject m = new JObject();
            m = JsonConvert.DeserializeObject<JObject>(content);
            if (content.Contains("host"))
                host = (string)m.GetValue("host");
            if (content.Contains("port"))
                port = (string)m.GetValue("port");
            if (content.Contains("head"))
                head = (string)m.GetValue("head");
        }

        //获取本机Mac地址
        private string GetLocalMac()
        {
            try
            {
                StringBuilder localMac = new StringBuilder();
                NetworkInterface[] n = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface adapter in n)
                {
                    PhysicalAddress mac = adapter.GetPhysicalAddress();
                    byte[] bytes = mac.GetAddressBytes();//返回当前实例的地址

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("X2"));//以十六进制格式化
                                                            /*if (i != bytes.Length - 1)
                                                            {
                                                                Console.WriteLine("content is {0}", sb);
                                                                sb.Append("-");
                                                            }*/
                    }

                    if (sb.Length - 1 == 11)
                    {
                        localMac = sb;
                    }
                }

                return localMac.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //http请求发送函数
        private string HttpSend(string url)
        {
            string result;
            using (HttpClient http = new HttpClient())
            {
                try
                {
                    var content = http.GetAsync(url);
                    var rep = content.Result;
                    var ret = rep.Content.ReadAsStringAsync();
                    result = ret.Result;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return result;
        }

        //探测是否升级
        private void ProbeUpgradeCmd()
        {
            string url = head + host + ":" + port + "/game/sdk_upgrade?devid=" + mac;
            while (true)
            {
                if (state == 0)
                {
                    string result = HttpSend(url);
                    Console.WriteLine("http send url {0}", url);
                    if (result != null && result.Contains("ok"))
                    {
                        //调用下载命令
                        state = 1;
                        try
                        {
                            Console.WriteLine("开始下载......");
                            DownloadAndUpgrade();
                        }
                        catch (Exception)
                        {
                            state = 0;
                        }
                    }
                }

                Thread.Sleep(5000);
            }
        }

        //下载升级（其实是替换DLL文件）
        private void DownloadAndUpgrade()
        {
            string url = head + host + ":" + port + "/game/upgrade/SDK.zip";
            DownloadFile(url);
            while (state == 1)
            {
                Thread.Sleep(1000);
            }
        }

        public void DownloadFile(string url)
        {
            try
            {
                string myStringWebResource = null;
                string fileName = url.Substring(url.LastIndexOf("/") + 1);
                filename = "..\\" + fileName;
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback2);
                myWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
                myStringWebResource = url;
                if (!myStringWebResource.Contains("http://"))
                {
                    myStringWebResource = "http://" + myStringWebResource;
                }

                myWebClient.DownloadFileAsync(new Uri(myStringWebResource), filename);
            }
            catch (Exception e)
            {
                state = 0;
                throw e;
            }
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
        }

        private void DownloadFileCallback2(object sender, AsyncCompletedEventArgs e)
        {
            //下载完成，可以开始升级
            bool b = ExtractSdk();
            if (!b)
            {
                state = 0;
            }
            else
            {
                Console.WriteLine("替换原有文件");
                bool n = UpgradeSdk();
                if (!n)
                {
                    state = 0;
                }
            }
        }

        //解压函数
        private bool ExtractSdk()
        {
            try
            {
                Console.WriteLine("开始解压缩zip文件......升级中");
                try
                {
                    string n = filename.Substring(filename.LastIndexOf("\\") + 1, filename.LastIndexOf(".") - filename.LastIndexOf("\\") - 1);
                    Console.WriteLine("判断文件是否存在 {0}", Directory.Exists("..\\" + n));
                    if (Directory.Exists("..\\" + n))
                    {
                        Directory.Delete("..\\" + n, true);
                    }
                    ZipFile.ExtractToDirectory(filename, "..\\");
                }
                catch (InvalidDataException e)
                {
                    Console.WriteLine("升级失败 {0}......压缩包已损坏，请重新升级", e);
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("解压升级包失败......{0}", e);
                    return false;
                }
                Console.WriteLine("升级完成......删除压缩文件");
                Thread.Sleep(500);
                File.Delete(filename);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("解压升级包失败......{0}", e);
                return false;
            }
        }

        //升级（替换原有文件）
        private bool UpgradeSdk()
        {
            string n = filename.Substring(filename.LastIndexOf("\\") + 1, filename.LastIndexOf(".") - filename.LastIndexOf("\\") - 1);
            Console.WriteLine("in {0}, {1}, {2}", Directory.Exists("..\\" + n), Environment.CurrentDirectory, "..\\" + n);
            if (Directory.Exists("..\\" + n))
            {
                Console.WriteLine("if {0}");
                DirectoryInfo folder = new DirectoryInfo("..\\" + n);
                foreach (FileInfo file in folder.GetFiles())
                {
                    string path = "..\\daemon\\" + file.ToString();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    Console.WriteLine("替换文件成功{0}, {1}", file.FullName, path);
                    File.Copy(file.FullName, path, true);
                }

                return true;
            }

            return false;
        }
    }
}


