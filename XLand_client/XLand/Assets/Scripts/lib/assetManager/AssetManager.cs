using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

using xy3d.tstd.lib.wwwManager;
using xy3d.tstd.lib.assetBundleManager;
using xy3d.tstd.lib.systemIO;

#if USE_ASSETBUNDLE

#else

using UnityEditor;

#endif

namespace xy3d.tstd.lib.assetManager
{

	public class AssetManager
	{
		public static readonly bool LOADASYNC = true;

		private static string dataName = "ab.dat";

		private static AssetManager _Instance;

		public static AssetManager Instance {

			get {

				if (_Instance == null) {

					_Instance = new AssetManager ();
				}

				return _Instance;
			}
		}

#if USE_ASSETBUNDLE
		
		public Dictionary<string,IAssetManagerUnit> dic;
		
		private GameObject go;
		public AssetManagerScript script;

		private Dictionary<string,AssetUnitData> dataDic;

		public AssetManager ()
		{
			dic = new Dictionary<string, IAssetManagerUnit> ();

			if(LOADASYNC){
				
				go = new GameObject("AssetManagerGameObject");

				GameObject.DontDestroyOnLoad(go);
				
				script = go.AddComponent<AssetManagerScript>();
			}
		}

		public void Init(Action _callBack){

			Action<Dictionary<string,AssetUnitData>> callBack = delegate(Dictionary<string,AssetUnitData> _data) {
				
				LoadAssetData(_data,_callBack);
			};

			SystemIO.LoadSerializeFileWithWWW(dataName,callBack);
		}

		private void LoadAssetData (Dictionary<string,AssetUnitData> _data, Action _callBack)
		{
			dataDic = _data;
				
			if(_callBack != null){

				_callBack();
			}
		}

		public AssetUnitData GetData (string _name)
		{
			string name = _name.ToLower();
			
			if(!dataDic.ContainsKey(name)){
				
				SuperDebug.LogError("AssetBundle中没有找到Asset:" + _name);
			}
			
			return dataDic [name];
		}

		public void RemoveUnit (string _name)
		{
			
			dic.Remove (_name);
		}
#endif

		public static void CreateAssetData(List<string> assetNames,List<string> assetBundleNames,Dictionary<string,List<string>> result){

			Dictionary<string,AssetUnitData> dic = new Dictionary<string, AssetUnitData> ();

			for (int i = 0; i < assetNames.Count; i++) {

				AssetUnitData unit = new AssetUnitData();
				
				unit.assetBundle = assetBundleNames[i];
				
				List<string> tmpList = result[assetNames[i]];
				
				unit.assetBundleDep = tmpList.ToArray();
				
				dic.Add(assetNames[i],unit);
			}

			SystemIO.SaveSerializeFile(Application.streamingAssetsPath + "/" + dataName,dic);
		}

		public void GetAsset<T> (string _name, Action<T> _callBack) where T:UnityEngine.Object
		{

#if USE_ASSETBUNDLE

			AssetManagerUnit<T> unit;
			
			if (!dic.ContainsKey (_name)) {
				
				unit = new AssetManagerUnit<T> (_name);
				
				dic.Add(_name,unit);
				
			} else {
				
				unit = dic [_name] as AssetManagerUnit<T>;
			}
			
			unit.Load (_callBack);

#else

			T data = AssetDatabase.LoadAssetAtPath<T> (_name);

			if(data == null){

				SuperDebug.LogError("Resource load fail:" + _name);
			}

			_callBack (data);
#endif
		}
	}
}
