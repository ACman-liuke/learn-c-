using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace killprocess
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.ProcessCtrl();
            Console.ReadKey();
        }

        private void ProcessCtrl()
        {
            DirectoryInfo folder1 = new DirectoryInfo("..\\SDK游戏控制器");
            foreach (FileInfo f in folder1.GetFiles())
            {
                if (f.Extension == ".exe")
                {
                    string ProcessName = f.ToString().Substring(0, f.ToString().LastIndexOf("."));
                    Process[] p = Process.GetProcessesByName(ProcessName);
                    Console.WriteLine("---process name---{0},{1}", ProcessName, p.Length);
                    foreach (Process pro in p)
                    {
                        Console.WriteLine("---in---for---{0}", pro);
                        pro.Kill();
                        break;
                    }

                    Process.Start(f.FullName);
                }
            }
        }
     }
}
