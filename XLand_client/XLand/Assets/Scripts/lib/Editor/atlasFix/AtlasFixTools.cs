using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AtlasFixTools{

	[MenuItem("图集修正/将所有Image的Sprite指向AtlasData")]
	public static void Start(){

		List<string> paths = new List<string>();

		string[] strs = AssetDatabase.FindAssets("t:sprite");

		for(int i = 0 ; i < strs.Length ; i++){
			
			string path = AssetDatabase.GUIDToAssetPath(strs[i]);
			
			bool skip = false;
			
			foreach(string kk in CodeHotFixTools.ignorePaths){
				
				if(path.IndexOf(kk) != -1){
					
					skip = true;
					
					break;
				}
			}
			
			if(!skip){
				
				paths.Add(path);
			}
		}

		List<string> atlasNames = new List<string>();

		List<AtlasData> atlasDatas = new List<AtlasData>();

		Dictionary<string,AtlasData> atlasDic = new Dictionary<string, AtlasData>();

		foreach(string path in paths){

			TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;

			if(!string.IsNullOrEmpty(ti.spritePackingTag)){

				AtlasData atlasData;

				if(!atlasNames.Contains(ti.spritePackingTag)){

					atlasNames.Add(ti.spritePackingTag);

					atlasData = new AtlasData();

					atlasDatas.Add(atlasData);

					atlasData.atlasName = ti.spritePackingTag;


				}else{

					int atlasIndex = atlasNames.IndexOf(ti.spritePackingTag);

					atlasData = atlasDatas[atlasIndex];
				}

				Array.Resize(ref atlasData.names,atlasData.names.Length + 1);

				atlasData.names[atlasData.names.Length - 1] = path;

				Array.Resize(ref atlasData.sprites,atlasData.sprites.Length + 1);
				
				atlasData.sprites[atlasData.sprites.Length - 1] = AssetDatabase.LoadAssetAtPath<Sprite>(path);

				atlasDic.Add(path,atlasData);
			}
		}

		foreach(AtlasData atlasData in atlasDatas){

			AssetDatabase.CreateAsset(atlasData,"Assets/Arts/ui/atlas/" + atlasData.atlasName + ".asset");
			
			AssetImporter import = AssetImporter.GetAtPath("Assets/Arts/ui/atlas/" + atlasData.atlasName + ".asset");
			
			import.assetBundleName = atlasData.atlasName;
		}

//		return;

		strs = AssetDatabase.FindAssets("t:prefab");
		
		for(int i = 0 ; i < strs.Length ; i++){
			
			string path = AssetDatabase.GUIDToAssetPath(strs[i]);
			
			bool skip = false;
			
			foreach(string kk in CodeHotFixTools.ignorePaths){
				
				if(path.IndexOf(kk) != -1){
					
					skip = true;
					
					break;
				}
			}
			
			if(!skip){
				
				GameObject resource = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				Check(atlasDic,resource);
			}
		}
	}

	private static void Check(Dictionary<string,AtlasData> atlasDic, GameObject go){

		Image img = go.GetComponent<Image>();

		if(img != null){

			if(img.sprite != null){
				
				string spPath = AssetDatabase.GetAssetPath(img.sprite);
				
				if(atlasDic.ContainsKey(spPath)){
					
					img.sprite = null;
					
					AtlasImageScript script = img.gameObject.AddComponent<AtlasImageScript>();
					
					script.atlasData = atlasDic[spPath];
					
					script.spriteName = spPath;
				}
			}
		}

		Toggle toggle = go.GetComponent<Toggle>();
		
		if(toggle != null){

			SpriteState spriteState = toggle.spriteState;
			
			toggle.spriteState = new SpriteState();
			
			AtlasToggleScript script = toggle.gameObject.AddComponent<AtlasToggleScript>();
			
			if(spriteState.disabledSprite != null){
				
				string spPath = AssetDatabase.GetAssetPath(spriteState.disabledSprite);
				
				script.disabledSpriteAtlasData = atlasDic[spPath];
				
				script.disabledSpriteName = spPath;
			}
			
			if(spriteState.highlightedSprite != null){
				
				string spPath = AssetDatabase.GetAssetPath(spriteState.highlightedSprite);
				
				script.highlightedSpriteAtlasData = atlasDic[spPath];
				
				script.highlightedSpriteName = spPath;
			}
			
			if(spriteState.pressedSprite != null){
				
				string spPath = AssetDatabase.GetAssetPath(spriteState.pressedSprite);
				
				script.pressedSpriteAtlasData = atlasDic[spPath];
				
				script.pressedSpriteName = spPath;
			}
		}

		for(int i = 0 ; i < go.transform.childCount ; i++){

			Check(atlasDic,go.transform.GetChild(i).gameObject);
		}
	}

	[MenuItem("图集修正/修改所有非九宫格的Image类型为Simple")]
	public static void Start2(){

		List<string> paths = new List<string>();
		
		string[] strs = AssetDatabase.FindAssets("t:prefab");
		
		for(int i = 0 ; i < strs.Length ; i++){
			
			string path = AssetDatabase.GUIDToAssetPath(strs[i]);
			
			bool skip = false;
			
			foreach(string kk in CodeHotFixTools.ignorePaths){
				
				if(path.IndexOf(kk) != -1){
					
					skip = true;
					
					break;
				}
			}
			
			if(!skip){
				
				GameObject resource = AssetDatabase.LoadAssetAtPath<GameObject>(path);

				Image[] imgs = resource.GetComponentsInChildren<Image>();

				foreach(Image img in imgs){

					if(img.sprite != null && img.type == Image.Type.Sliced){

						 Rect rect = (img.gameObject.transform as RectTransform).rect;

						if(rect.width == img.sprite.rect.width && rect.height == img.sprite.rect.height){

							img.type = Image.Type.Simple;
						}
					}
				}
			}
		}
	}
}
