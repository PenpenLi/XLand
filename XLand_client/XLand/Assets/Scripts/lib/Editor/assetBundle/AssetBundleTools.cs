using UnityEngine;
using System.Collections;
using UnityEditor;

using System;
using xy3d.tstd.lib.assetManager;
using System.Collections.Generic;
using xy3d.tstd.lib.assetBundleManager;
using System.IO;
using System.Reflection;
using UnityEngine.Rendering;

public class AssetBundleTools{

	[MenuItem("AssetBundle/清除所有选中对象的AssetBundle名字")]
	public static void ClearSelectedAssetBundleName(){

		UnityEngine.Object[] objects = Selection.objects;
		
		foreach(UnityEngine.Object obj in objects){
			
			string path = AssetDatabase.GetAssetPath(obj);

			SetAssetBundleName(path,null);

			AssetDatabase.RemoveUnusedAssetBundleNames();
		}
	}

	[MenuItem("AssetBundle/设置所有选中对象的AssetBundle名字")]
	public static void SetSelectedAssetBundleName(){
		
		UnityEngine.Object[] objects = Selection.objects;
		
		foreach(UnityEngine.Object obj in objects){
			
			string path = AssetDatabase.GetAssetPath(obj);

			SetAssetBundleName(path,obj.name);
		}
	}

	public static string GetAssetBundleName(string _path){

		AssetImporter importer = AssetImporter.GetAtPath(_path);

		return importer.assetBundleName;
	}

	public static void SetAssetBundleName(string _path,string _name){

		AssetImporter importer = AssetImporter.GetAtPath(_path);

		importer.assetBundleName = _name;
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:PC")]
	public static void CreateAssetBundlePC(){

		AssetBundleManifest manifest = CreateAssetBundle(BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.StandaloneWindows64);
		
		CreateAssetBundleDat(manifest,false);
		
		CreateAssetBundle(BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.StandaloneWindows64);

		Debug.Log("AssetBundle生成成功！");
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:IOS")]
	public static void CreateAssetBundleIOS(){
		
		AssetBundleManifest manifest = CreateAssetBundle(BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.iOS);
		
		CreateAssetBundleDat(manifest,false);
		
		CreateAssetBundle(BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.iOS);

		Debug.Log("AssetBundle生成成功！");
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:Android")]
	public static void CreateAssetBundleAndroid(){

		AssetBundleManifest manifest = CreateAssetBundle(BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.Android);

		CreateAssetBundleDat(manifest,false);

		CreateAssetBundle(BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.Android);

		Debug.Log("AssetBundle生成成功！");
	}
	
	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:Android热更新")]
	public static void CreateAssetBundleAndroidUseAssembly(){
		
		AssetBundleManifest manifest = CreateAssetBundle(BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.Android);
		
		CreateAssetBundleDat(manifest,true);
		
		CreateAssetBundle(BuildAssetBundleOptions.ForceRebuildAssetBundle,BuildTarget.Android);
		
		Debug.Log("AssetBundle生成成功！");
	}

	private static void PrepareToBuildAssetBundle(){

		DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/assetbundle");

		FileInfo[] fileInfos = directoryInfo.GetFiles();

		foreach(FileInfo fileInfo in fileInfos){

			fileInfo.Delete();
		}
	}

	private static AssetBundleManifest CreateAssetBundle(BuildAssetBundleOptions _option,BuildTarget _buildTarget){
		
		PrepareToBuildAssetBundle();

		RenderSettings.fog = true;
		
		RenderSettings.fogMode = FogMode.Linear;

		LightmapData[] lightMaps = new LightmapData[1];

		lightMaps[0] = new LightmapData();

		lightMaps[0].lightmapFar = new Texture2D(100,100);

		LightmapSettings.lightmaps = lightMaps;

		LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;

		AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/" + AssetBundleManager.path,_option,_buildTarget);

		RenderSettings.fog = false;

		LightmapSettings.lightmaps = new LightmapData[0];

		return manifest;
	}

	private static void CreateAssetBundleDat(AssetBundleManifest manifest,bool _useAssembly){

		if(manifest == null){

			return;
		}

		string[] abs = manifest.GetAllAssetBundles ();
		
		AssetBundle[] aaaa = new AssetBundle[abs.Length];
		
		try{
			
			List<UnityEngine.Object> assets = new List<UnityEngine.Object> ();
			
			List<string> assetNames = new List<string> ();
			
			List<string> assetBundleNames = new List<string> ();

			Dictionary<string,List<string>> result = new Dictionary<string, List<string>> ();
			
			for(int i = 0 ; i < abs.Length ; i++){

				AssetBundle ab = AssetBundle.CreateFromFile(Application.streamingAssetsPath + "/" + AssetBundleManager.path + abs[i]);
				
				aaaa[i] = ab;
				
				string[] nn = ab.GetAllAssetNames();
				
				foreach(string str in nn){
					
					if(assetNames.Contains(str)){
						
						SuperDebug.LogError("error!");
						
					}else{
						
						assetNames.Add(str);
						
						UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(str);
						
						assets.Add(obj);
						
						assetBundleNames.Add(abs[i]);

						List<string> ll = new List<string>();
						
						result.Add(str,ll);
					}
				}
			}
			
			for (int i = 0; i < assetNames.Count; i++) {
				
				string name = assetNames[i];
				UnityEngine.Object obj = assets[i];
				List<string> list = result[name];
				
				UnityEngine.Object[] sss = EditorUtility.CollectDependencies(new UnityEngine.Object[]{obj});
				
				foreach(UnityEngine.Object dd in sss){
					
					if(dd != obj){
						
						if(assets.Contains(dd)){
							
							string assetBundleName = assetBundleNames[assets.IndexOf(dd)];
							
							if(!list.Contains(assetBundleName)){
								
								list.Add(assetBundleName);
							}
						}
					}
				}
			}

			if(_useAssembly){

				FileStream fs = new FileStream(Application.streamingAssetsPath + "/" + AssemblyShell.ASSEMBLY_FILE_NAME,FileMode.Open);
				
				byte[] bytes = new byte[fs.Length];
				
				fs.Read(bytes,0,(int)fs.Length);
				
				fs.Close();
				
				Assembly ass = Assembly.Load(bytes);
				
				Type type = ass.GetType("xy3d.tstd.lib.assetManager.AssetManager");

				type.GetMethod("CreateAssetData").Invoke(null,new object[]{assetNames,assetBundleNames,result});

			}else{

				AssetManager.CreateAssetData(assetNames,assetBundleNames,result);
			}

		}catch(Exception e){
			
			Debug.Log("error:" + e.Message);
			
		}finally{
			
			foreach (AssetBundle aaa in aaaa) {
				
				aaa.Unload (true);
			}
		}
	}

	[MenuItem("AssetBundle/清除所有AssetBundle设置  千万不要乱点！！！")]
	public static void ClearAllAssetBundleName(){
		
		string[] names = AssetDatabase.GetAllAssetBundleNames();
		
		foreach(string assetBundleName in names){
			
			AssetDatabase.RemoveAssetBundleName(assetBundleName,true);
		}
	}
}
