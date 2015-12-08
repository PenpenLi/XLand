using UnityEngine;
using System.Collections;
using System.IO;
using xy3d.tstd.lib.publicTools;

public class RecordLog{

	private static StreamWriter sw;

	private static bool isStart = false;

	[RuntimeInitializeOnLoadMethod]
	public static void Start(){

		GameObject go = new GameObject("RecordLogGameObejct",typeof(RecordLogScript));

		Application.logMessageReceived += UnityLogCallback;

		if(File.Exists(Application.persistentDataPath + "/log.dat")){

			File.Delete(Application.persistentDataPath + "/log.dat");
		}

		sw = new StreamWriter(Application.persistentDataPath + "/log.dat");

		isStart = true;
	}

	private static void UnityLogCallback(string condition, string stackTrace, LogType type)
	{
		string str = "";

		switch (type) {
			
		case LogType.Assert:
		case LogType.Error:
		case LogType.Exception:
		case LogType.Warning:
			str = string.Format("{0}, {1}",condition,stackTrace);
			break;

		case LogType.Log:
//			str = string.Format("{0}",condition);
			return;
			
		}

		Write(str);
	}

	public static void Write(string _str){

		if(isStart){

			sw.WriteLine(_str);
			
			sw.Flush();
		}
	}

	public static void Stop(){

		isStart = false;

		sw.Close();
	}
}
