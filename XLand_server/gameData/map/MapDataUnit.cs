using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XLand_server.gameData.map
{
    class MapDataUnit
    {
        public int mapWidth;
        public int mapHeight;

        public int score1;
        public int score2;

        public Dictionary<int, int> dic = new Dictionary<int, int>();

        public Dictionary<int, int[]> neighbourDic = new Dictionary<int, int[]>();

        public void Init(Stream _stream)
        {
            BinaryReader reader = new BinaryReader(_stream);

            mapWidth = reader.ReadInt16();

            mapHeight = reader.ReadInt16();

            int index = 0;

            for (int i = 0; i < mapHeight; i++)
            {
                if (i % 2 == 0)
                {
                    for (int m = 0; m < mapWidth; m++)
                    {
                        int type = reader.ReadInt16();

                        if(type == 1)
                        {
                            dic.Add(index, type);

                            score1++;
                        }
                        else if(type == 2)
                        {
                            dic.Add(index, type);

                            score2++;
                        }

                        index++;
                    }
                }
                else
                {
                    for (int m = 0; m < mapWidth - 1; m++)
                    {
                        int type = reader.ReadInt16();

                        if (type == 1)
                        {
                            dic.Add(index, type);

                            score1++;
                        }
                        else if (type == 2)
                        {
                            dic.Add(index, type);

                            score2++;
                        }

                        index++;
                    }
                }
            }

            int size = mapWidth * mapHeight - mapHeight / 2;

            Dictionary<int, int>.Enumerator enumerator = dic.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                int pos = enumerator.Current.Key;
                
                int[] vec = GetNeighbourPosVec(pos,size);

                neighbourDic.Add(pos, vec);
            }
        }

        private int[] GetNeighbourPosVec(int _pos,int _size)
        {
            int[] vec = new int[6];

            if (_pos % (mapWidth * 2 - 1) != 0)
            {
                if (_pos > mapWidth - 1)
                {
                    int p = _pos - mapWidth;

                    if (dic.ContainsKey(p))
                    {
                        vec[5] = p;
                    }
                    else
                    {
                        vec[5] = -1;
                    }
                }
                else
                {
                    vec[5] = -1;
                }

                if (_pos < _size - mapWidth)
                {
                    int p = _pos + mapWidth - 1;

                    if (dic.ContainsKey(p))
                    {
                        vec[3] = p;
                    }
                    else
                    {
                        vec[3] = -1;
                    }
                }
                else
                {
                    vec[3] = -1;
                }

                if (_pos % (mapWidth * 2 - 1) != mapWidth)
                {
                    int p = _pos - 1;

                    if (dic.ContainsKey(p))
                    {
                        vec[4] = p;
                    }
                    else
                    {
                        vec[4] = -1;
                    }

                }
                else
                {
                    vec[4] = -1;
                }

            }
            else
            {
                vec[3] = -1;
                vec[4] = -1;
                vec[5] = -1;
            }

            if (_pos % (mapWidth * 2 - 1) != mapWidth - 1)
            {
                if (_pos > mapWidth - 1)
                {
                    int p = _pos - mapWidth + 1;

                    if (dic.ContainsKey(p))
                    {
                        vec[0] = p;
                    }
                    else
                    {
                        vec[0] = -1;
                    }
                }
                else
                {
                    vec[0] = -1;
                }

                if (_pos < _size - mapWidth)
                {
                    int p = _pos + mapWidth;

                    if (dic.ContainsKey(p))
                    {
                        vec[2] = p;
                    }
                    else
                    {
                        vec[2] = -1;
                    }

                }
                else
                {
                    vec[2] = -1;
                }

                if (_pos % (mapWidth * 2 - 1) != mapWidth * 2 - 2)
                {
                    int p = _pos + 1;

                    if (dic.ContainsKey(p))
                    {
                        vec[1] = p;
                    }
                    else
                    {
                        vec[1] = -1;
                    }

                }
                else
                {
                    vec[1] = -1;
                }

            }
            else
            {
                vec[0] = -1;
                vec[1] = -1;
                vec[2] = -1;
            }

            return vec;
        }
    }
}
