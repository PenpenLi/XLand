using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperServer.csv;

namespace XLand_server.gameData.csv
{
    class CsvInit
    {
        public static void Init(string _path)
        {
            StaticData.Instance.Init(_path + @"/csv/");

            StaticData.Instance.Load<MapCsv>("map");
        }
    }
}
