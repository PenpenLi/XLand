using System.Collections.Generic;
using XLand_server.gameData.csv;
using SuperServer.csv;
using System.IO;

namespace XLand_server.gameData.map
{
    class MapData
    {
        public static Dictionary<int, MapDataUnit> dic = new Dictionary<int, MapDataUnit>();

        public static void Init(string _path)
        {
            Dictionary<int, MapCsv> csvDic = StaticData.Instance.GetDic<MapCsv>();

            Dictionary<int, MapCsv>.Enumerator enumerator = csvDic.GetEnumerator();

            while (enumerator.MoveNext())
            {
                MapCsv csv = enumerator.Current.Value;

                using (FileStream fs = new FileStream(_path + "/map/" + csv.mapName, FileMode.Open))
                {
                    MapDataUnit unit = new MapDataUnit();

                    unit.Init(fs);

                    dic.Add(enumerator.Current.Key, unit);
                }
            }
        }
    }
}
