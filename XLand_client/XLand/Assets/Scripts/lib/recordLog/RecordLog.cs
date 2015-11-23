using UnityEngine;
using System.Collections;
using System.IO;
using xy3d.tstd.lib.publicTools;

public class RecordLog{

	private static StreamWriter sw;

	public static void Start(){

		Application.logMessageReceived += UnityLogCallback;

		if(File.Exists(Application.persistentDataPath + "/log.dat")){

			File.Delete(Application.persistentDataPath + "/log.dat");
		}

		sw = new StreamWriter(Application.persistentDataPath + "/log.dat");
	}

	private static void UnityLogCallback(string condition, string stackTrace, LogType type)
	{
		string str = "";

		switch (type) {
			
		case LogType.Assert:
		case LogType.Error:
		case LogType.Exception:
			str = string.Format("{0}, {1}",condition,stackTrace);
			break;
			
		case LogType.Warning:
			str = string.Format("{0}, {1}",condition,stackTrace);
			break;
			
		case LogType.Log:
			str = string.Format("{0}",condition);
			break;
			
		}

		Write(str);
	}

	public static void Write(string _str){

		sw.WriteLine(_str);
		
		sw.Flush();
	}

	public static void Stop(){

		sw.Close();
	}
}
