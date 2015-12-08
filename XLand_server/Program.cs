using SuperServer.redis;
using SuperServer.timer;
using SuperServer.server;
using XLand_server.userData;
using XLand_server.userService;
using XLand_server.gameData;
using System;
using SuperServer.csv;
using XLand_server.gameData.map;

namespace XLand_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer.Instance.Start(100);

            Redis.Instance.Start("127.0.0.1", 6379);

            Server.Instance.Start<XLandUserService, XLandUserData>("127.0.0.1", 1983, 1000);

            GameData.Init(@"C:/UnityProjects/XLand/XLand_data");

            StaticData s = StaticData.Instance;

            var dic = MapData.dic;

            Console.ReadKey();
        }
    }
}
