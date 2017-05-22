using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Threading;

namespace soundtest
{
    class Program
    {
        private static string filename = "test.wav";
        private static SoundPlayer s = new SoundPlayer(filename);

        static void Main(string[] args)
        {
            if (File.Exists(filename))
            {
                Console.WriteLine("准备播放声音文件");
                s.Play();
                Console.WriteLine("播放声音文件成功");
                Thread.Sleep(30000);
                s.Stop();
                Console.WriteLine("播放声音完成");
            }
            s.Play();
            Console.ReadKey();
        }
    }
}
