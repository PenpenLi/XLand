using UnityEngine;
using System.Collections;
using System;

public class AssemblyManagerDownLoader : MonoBehaviour {

	private Action<byte[]> callBack;

	private bool loadOK = false;

	private byte[] bytes;

	public void Init(string _path,Action<byte[]> _callBack){

		SuperDebug.Log("加载代码文件:" + _path);

		callBack = _callBack;

		StartCoroutine(LoadDll(_path));
	}

	void Update(){

		if(loadOK){

			callBack(bytes);

			GameObject.Destroy(gameObject);
		}
	}

	IEnumerator LoadDll(string _path){

		WWW www = new WWW(_path);

		yield return www;

		loadOK = true;

		if(string.IsNullOrEmpty(www.error)){

			bytes = www.bytes;

		}else{

			SuperDebug.Log(www.error);
		}
	}
}
