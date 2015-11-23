using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class TexturePackageAtlasTools
{

    [MenuItem("图集修正/将所有Image的Sprite指向TexturePackage的AtlasData")]
    public static void Start()
    {

        List<string> paths = new List<string>();

        string[] strs = AssetDatabase.FindAssets("t:sprite");

        for (int i = 0; i < strs.Length; i++)
        {

            string path = AssetDatabase.GUIDToAssetPath(strs[i]);

            bool skip = false;

            foreach (string kk in CodeHotFixTools.ignorePaths)
            {

                if (path.IndexOf(kk) != -1)
                {

                    skip = true;

                    break;
                }
            }

            if (!skip)
            {

                paths.Add(path);
            }
        }

        Dictionary<string, Sprite> atlasDic = new Dictionary<string, Sprite>();
        List<string> pathList = new List<string>();
        Dictionary<string, TextureImporter> textureImporterDic = new Dictionary<string, TextureImporter>();
        Dictionary<TextureImporter, string> textureImporterPathDic = new Dictionary<TextureImporter, string>();

        Dictionary<string, Sprite> singleDic = new Dictionary<string, Sprite>();


        TextureImporter tempTI = null;
        string tempPath = "";

        foreach (string path in paths)
        {

            TextureImporter ti = TextureImporter.GetAtPath(path) as TextureImporter;

            if (ti.spriteImportMode == SpriteImportMode.Multiple && !pathList.Contains(path))
            {
                pathList.Add(path);

                tempTI = ti;
                tempPath = path;
                textureImporterPathDic.Add(ti, path);
                object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);

                for (int index = 0; index < objs.Length; index++)
                {
                    object obj = objs[index];
                    if (obj is Sprite)
                    {
                        Sprite sprite = obj as Sprite;

                        if (!atlasDic.ContainsKey(sprite.name))
                        {
                            atlasDic.Add(sprite.name, sprite);
                            textureImporterDic.Add(sprite.name, ti);
                        }
                    }
                }

            }
            else
            {
                if (ti.spriteImportMode != SpriteImportMode.Multiple)
                {
                    Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    singleDic.Add(s.name, s);
                }
                
            }

        }

        //return;

        strs = AssetDatabase.FindAssets("t:prefab");

        for (int i = 0; i < strs.Length; i++)
        {

            string path = AssetDatabase.GUIDToAssetPath(strs[i]);

            bool skip = false;

            foreach (string kk in CodeHotFixTools.ignorePaths)
            {

                if (path.IndexOf(kk) != -1)
                {

                    skip = true;

                    break;
                }
            }

            if (!skip)
            {

                GameObject resource = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                bool isChange = false;
                Check(atlasDic, textureImporterDic, textureImporterPathDic, resource, ref isChange);
                if (isChange)
                {
                    EditorUtility.SetDirty(resource);
                }

            }
        }
        SaveMeta(textureImporterPathDic, singleDic);
        EditorApplication.SaveScene(EditorApplication.currentScene);
        Debug.Log("图集修正完成!!!!!");
    }

    private static void SaveMeta(Dictionary<TextureImporter, string> textureImporterPathDic, Dictionary<string, Sprite> singleDic)
    {
        foreach (TextureImporter ti in textureImporterPathDic.Keys)
        {
            string path = textureImporterPathDic[ti];

            for (int index = 0; index < ti.spritesheet.Length; index++)
            {

                if (singleDic.ContainsKey(ti.spritesheet[index].name))
                {
                    Sprite s = singleDic[ti.spritesheet[index].name];
                    SpriteMetaData[] newBorderList = (SpriteMetaData[])ti.spritesheet.Clone();
                    newBorderList[index].border = s.border;
                    ti.spritesheet = newBorderList;
                }

            }

            EditorUtility.SetDirty(ti);
            AssetDatabase.ImportAsset(path);
        }
    }

    private static void Check(Dictionary<string, Sprite> atlasDic, Dictionary<string, TextureImporter> textureImporterDic, Dictionary<TextureImporter, string> textureImporterPathDic, GameObject go, ref bool isChange)
    {
        Image img = go.GetComponent<Image>();

        if (img != null)
        {

            if (img.sprite != null)
            {

                if (atlasDic.ContainsKey(img.sprite.name))
                {
                    //Sprite s = img.sprite;
                    img.sprite = atlasDic[img.sprite.name];

                    //TextureImporter ti = textureImporterDic[s.name];
                    //for(int index = 0; index < ti.spritesheet.Length; index ++)
                    //{

                    //    if (ti.spritesheet[index].name == s.name)
                    //    {
                    //        SpriteMetaData[] newBorderList = (SpriteMetaData[])ti.spritesheet.Clone();
                    //        newBorderList[index].border = s.border;
                    //        ti.spritesheet = newBorderList;
                    //    }
                        
                    //}

                    isChange = true;
                }
            }
        }

        Toggle toggle = go.GetComponent<Toggle>();

        if (toggle != null)
        {

            SpriteState spriteState = toggle.spriteState;

            if (spriteState.disabledSprite != null)
            {

                if (atlasDic.ContainsKey(spriteState.disabledSprite.name))
                {
                    //Sprite s = spriteState.disabledSprite;
                    spriteState.disabledSprite = atlasDic[spriteState.disabledSprite.name];
                    isChange = true;
                    //TextureImporter ti = textureImporterDic[s.name];
                    //for (int index = 0; index < ti.spritesheet.Length; index++)
                    //{

                    //    if (ti.spritesheet[index].name == s.name)
                    //    {
                    //        SpriteMetaData[] newBorderList = (SpriteMetaData[])ti.spritesheet.Clone();
                    //        newBorderList[index].border = s.border;
                    //        ti.spritesheet = newBorderList;
                    //    }

                    //}
                }
            }

            if (spriteState.highlightedSprite != null)
            {

                if (atlasDic.ContainsKey(spriteState.highlightedSprite.name))
                {
                    //Sprite s = spriteState.highlightedSprite;
                    spriteState.highlightedSprite = atlasDic[spriteState.highlightedSprite.name];
                    isChange = true;
                    //TextureImporter ti = textureImporterDic[s.name];
                    //for (int index = 0; index < ti.spritesheet.Length; index++)
                    //{

                    //    if (ti.spritesheet[index].name == s.name)
                    //    {
                    //        SpriteMetaData[] newBorderList = (SpriteMetaData[])ti.spritesheet.Clone();
                    //        newBorderList[index].border = s.border;
                    //        ti.spritesheet = newBorderList;
                    //    }

                    //}
                }
            }

            if (spriteState.pressedSprite != null)
            {

                if (atlasDic.ContainsKey(spriteState.pressedSprite.name))
                {
                    //Sprite s = spriteState.pressedSprite;
                    spriteState.pressedSprite = atlasDic[spriteState.pressedSprite.name];
                    isChange = true;
                    //TextureImporter ti = textureImporterDic[s.name];
                    //for (int index = 0; index < ti.spritesheet.Length; index++)
                    //{

                    //    if (ti.spritesheet[index].name == s.name)
                    //    {
                    //        SpriteMetaData[] newBorderList = (SpriteMetaData[])ti.spritesheet.Clone();
                    //        newBorderList[index].border = s.border;
                    //        ti.spritesheet = newBorderList;
                    //    }

                    //}
                }
            }
        }


        for (int i = 0; i < go.transform.childCount; i++)
        {

            Check(atlasDic, textureImporterDic, textureImporterPathDic, go.transform.GetChild(i).gameObject, ref isChange);
        }

    }

}
