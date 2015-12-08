using System.IO;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.UI;
using xy3d.tstd.lib.superList;

public class PrefabCheckTools{

    [MenuItem("Prefab检测/删除多余的CanvasRenderer并检查是否有脚本丢失")]
    public static void SSS()
    {
		string[] strs = AssetDatabase.GetAllAssetPaths();

		strs = AssetDatabase.FindAssets("t:prefab");

		string[] paths = new string[strs.Length];

		for(int i = 0 ; i < strs.Length ; i++){

			paths[i] = AssetDatabase.GUIDToAssetPath(strs[i]);
		}

		foreach(string path in paths){

			bool skip = false;

			foreach(string kk in CodeHotFixTools.ignorePaths){

				if(path.IndexOf(kk) != -1){

					skip = true;

					break;
				}
			}

			if(!skip){

				Check(path);
			}
		}
    }

	private static void Check(string _path){

		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(_path);

		Fix(prefab);
	}

	private static void Fix(GameObject _go){

		CanvasRenderer r = _go.GetComponent<CanvasRenderer>();

		if(r != null){

			Image img = _go.GetComponent<Image>();

			Text text = _go.GetComponent<Text>();

			RawImage rawImg = _go.GetComponent<RawImage>();

			if(img == null && text == null && rawImg == null){

				GameObject.DestroyImmediate(r,true);
			}
		}

		MonoBehaviour[] b = _go.GetComponents<MonoBehaviour>();

		foreach(MonoBehaviour m in b){

			if(m == null){

				SuperDebug.LogErrorFormat("发现脚本丢失  root:{0}--->{1}",_go.transform.root.name,_go.name);

				break;
			}
		}

//		SuperList superList = _go.GetComponent<SuperList>();
//		
//		if(superList != null){
//
//			Mask mask = _go.GetComponent<Mask>();
//
//			if(mask != null){
//
//				GameObject.DestroyImmediate(mask);
//
//				Image img = _go.GetComponent<Image>();
//
//				GameObject.DestroyImmediate(img);
//
//				_go.AddComponent<RectMask2D>();
//
//				_hasChange = true;
//			}
//		}

		for(int i = 0 ; i < _go.transform.childCount ; i++){

			Fix(_go.transform.GetChild(i).gameObject);
		}
	}
}
