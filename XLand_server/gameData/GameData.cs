using XLand_server.gameData.csv;
using XLand_server.gameData.map;

namespace XLand_server.gameData
{
    class GameData
    {
        public static void Init(string _path)
        {
            CsvInit.Init(_path);

            MapData.Init(_path);
        }
    }
}
