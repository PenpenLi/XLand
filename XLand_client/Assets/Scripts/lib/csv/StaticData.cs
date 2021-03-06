﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using xy3d.tstd.lib.assetManager;
using xy3d.tstd.lib.wwwManager;
using xy3d.tstd.lib.systemIO;

public class StaticData
{
    public static string path;

    private static string datName = "csv.dat";

	private static Dictionary<Type, IDictionary> dic = new Dictionary<Type, IDictionary>();

    public static T GetData<T>(int _id) where T : CsvBase
    {
        Dictionary<int, T> tmpDic = dic[typeof(T)] as Dictionary<int, T>;

        if (!tmpDic.ContainsKey(_id))
        {
            SuperDebug.LogError(typeof(T).Name + "表中未找到ID为:" + _id + "的行!");
        }

        return tmpDic[_id];
    }

    public static T GetDataOfKey<T>(string key, object keyValueParam) where T : CsvBase
    {
        Dictionary<int, T> dict = GetDic<T>();

		Type type = typeof(T);

		FieldInfo field = type.GetField(key);

        foreach (T item in dict.Values)
        {
            object keyValue = field.GetValue(item);

            if (keyValue.Equals(keyValueParam))
            {
                return item;
            }
        }

        return default(T);
    }

    

    public static Dictionary<int, T> GetDic<T>() where T : CsvBase
    {
		Type type = typeof(T);

		if (!dic.ContainsKey(type))
		{
			SuperDebug.LogError("not find: " + type);
		}
		
		return dic[type] as Dictionary<int, T>;
    }

    public static List<T> GetList<T>() where T : CsvBase
    {
        Dictionary<int, T> dict = GetDic<T>();
        List<T> list = new List<T>();

        foreach (var item in dict)
        {
            list.Add(item.Value);
        }

        return list;
    }

	public static bool IsIDExists<T>(int _id)where T : CsvBase{

		Dictionary<int, T> dict = GetDic<T>();

		return dict.ContainsKey(_id);
	}

    public static void SaveCsvDataToFile()
    {
        SystemIO.SaveSerializeFile(Application.streamingAssetsPath + "/" + datName, dic);
    }

    public static void LoadCsvDataFromFile(Action _callBack)
    {
		Action<Dictionary<Type, IDictionary>> callBack = delegate(Dictionary<Type, IDictionary> obj) {

			LoadCsvDataFileOK(obj,_callBack);
		};

		SystemIO.LoadSerializeFileWithWWW(datName,callBack);
    }

	private static void LoadCsvDataFileOK(Dictionary<Type, IDictionary> _dic, Action _callBack)
    {
        dic = _dic;

        _callBack();
    }

    public static void Reset()
    {
        dic = new Dictionary<Type, IDictionary>();
    }

    public static void Load<T>(string _name) where T : CsvBase , new()
    {
        Type type = typeof(T);

        if (dic.ContainsKey(type))
        {
            return;
        }

        Dictionary<int, T> result = new Dictionary<int, T>();

		using (FileStream fs = new FileStream(path + "/" + _name + ".csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                int i = 0;

                string lineStr = reader.ReadLine();

                FieldInfo[] infoArr = null;

                while (lineStr != null)
                {
                    if (i == 2)
                    {
                        string[] dataArr = lineStr.Split(",".ToCharArray());

                        infoArr = new FieldInfo[dataArr.Length];

                        for (int m = 1; m < dataArr.Length; m++)
                        {
                            infoArr[m] = type.GetField(dataArr[m]);
                        }
                    }
                    else if (i > 2)
                    {
                        string[] dataArr = lineStr.Split(",".ToCharArray());

                        T csv = new T();

                        csv.ID = Int32.Parse(dataArr[0]);

                        for (int m = 1; m < infoArr.Length; m++)
                        {
                            FieldInfo info = infoArr[m];

                            if (info != null)
                            {
                                setData(info, csv, dataArr[m]);
                            }
                        }

						csv.Fix();

                        result.Add(csv.ID, csv);
                    }

                    i++;

                    lineStr = reader.ReadLine();
                }
            }
        }

        dic.Add(type, result);
        //        SuperDebug.Log("StaticData.Load" + Application.dataPath + path + _name + ".csv" + "  complete!!!");
    }

    private static void setData(FieldInfo _info, CsvBase _csv, string _data)
    {
        string str = "setData:" + _info.Name + "   " + _info.FieldType.Name + "   " + _data + "   " + _data.Length + Environment.NewLine;

        //SuperDebug.Log(str);
        try
        {
            switch (_info.FieldType.Name)
            {
                case "Int32":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, Int32.Parse(_data));
                    }

                    break;

                case "String":

                    _info.SetValue(_csv, _data);

                    break;

                case "Boolean":

                    _info.SetValue(_csv, _data == "1" ? true : false);

                    break;

                case "Single":

                    if (string.IsNullOrEmpty(_data))
                    {
                        _info.SetValue(_csv, 0);
                    }
                    else
                    {
                        _info.SetValue(_csv, float.Parse(_data));
                    }

                    break;

                case "Int32[]":

                    int[] intResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split("$".ToCharArray());

                        intResult = new int[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            intResult[i] = Int32.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        intResult = new int[0];
                    }

                    _info.SetValue(_csv, intResult);

                    break;

                case "String[]":

                    string[] stringResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        stringResult = _data.Split("$".ToCharArray());
                    }
                    else
                    {
                        stringResult = new string[0];
                    }

                    _info.SetValue(_csv, stringResult);

                    break;

                case "Boolean[]":

                    bool[] boolResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split("$".ToCharArray());

                        boolResult = new bool[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            boolResult[i] = strArr[i] == "1" ? true : false;
                        }
                    }
                    else
                    {
                        boolResult = new bool[0];
                    }

                    _info.SetValue(_csv, boolResult);

                    break;

                default:

                    float[] floatResult;

                    if (!string.IsNullOrEmpty(_data))
                    {
                        string[] strArr = _data.Split("$".ToCharArray());

                        floatResult = new float[strArr.Length];

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            floatResult[i] = float.Parse(strArr[i]);
                        }
                    }
                    else
                    {
                        floatResult = new float[0];
                    }

                    _info.SetValue(_csv, floatResult);

                    break;
            }
        }
        catch (Exception e)
        {
            SuperDebug.LogError(str + "   " + e.ToString());
        }
    }
}
