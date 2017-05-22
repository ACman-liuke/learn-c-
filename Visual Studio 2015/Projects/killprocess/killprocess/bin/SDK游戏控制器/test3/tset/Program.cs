using System;
using System.Threading;
using GmMessageApi;


namespace tset
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageApi n = new MessageApi();
            try
            {
                n.Init(args);
                n.Start();
                Console.WriteLine("---pass--start---");
                Thread.Sleep(1500);
                n.EventRcord("adnfajdjfasj");
                n.Commit();
                Thread.Sleep(1500);

                n.Result("test", "100", "1");
                Thread.Sleep(500);

                n.Stop();
                Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Console.WriteLine("test  code have a error is {0}", e);
                throw e;
            }
        }
    }
}
