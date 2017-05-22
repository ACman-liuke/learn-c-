using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using GmMessageApi;

namespace breakthrouthtest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.WriteLine("no param");
            }
            if (args.Length >=1 && args[0] != null)
                Console.WriteLine("get the first param is {0}", args[0]);
            if(args.Length >= 2 && args[1] != null)
                Console.WriteLine("get the second param is {0}", args[1]);
            if(args.Length >= 3 && args[2] != null)
                Console.WriteLine("get the third param is {0}", args[2]);
            if(args.Length >= 4 && args[3] != null)
                Console.WriteLine("get the fourth param is {0}", args[3]);
            
            Console.ReadKey();
        }
    }
}
