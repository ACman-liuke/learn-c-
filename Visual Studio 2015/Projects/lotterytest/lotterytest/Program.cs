using System;
using GmMessageApi;
using System.Threading;

namespace lotterytest
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageApi api = new MessageApi();
            MessageApi.KeyValueDelegate p = new MessageApi.KeyValueDelegate(getRank);
            api.SetRankMethod = getRank;
            api.Init(args);
            int n = api.GameCoinGet();
            Console.WriteLine("玩家下注游戏币 ====》》 {0}", n);
            api.Start();
            Thread.Sleep(5000);
            api.GameCoinSettle(1000);
            Thread.Sleep(1000);
            api.Stop();
            Thread.Sleep(20000);
        }

        private static void getRank(string key, string value)
        {
            Console.WriteLine("----get---Rank key={0}, value = {1}", key, value);
        }
    }
}
