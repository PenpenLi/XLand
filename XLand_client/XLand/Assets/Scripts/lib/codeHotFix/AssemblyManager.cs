using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.IO;

public class AssemblyManager{

	private static AssemblyManager _Instance;

	public static AssemblyManager Instance{

		get{

			if(_Instance == null){

				_Instance = new AssemblyManager();
			}

			return _Instance;
		}
	}

	public Assembly assembly;

	private Action<bool> callBack;

	public void Init(string _path,Action<bool> _callBack){

		callBack = _callBack;

		GameObject go = new GameObject();

		AssemblyManagerDownLoader downLoader = go.AddComponent<AssemblyManagerDownLoader>();

		downLoader.Init(_path,GetBytes);
	}

	private void GetBytes(byte[] _bytes){

		if(_bytes != null){

			assembly = Assembly.Load(_bytes);

			callBack(true);

		}else{

			callBack(false);
		}
	}
}
