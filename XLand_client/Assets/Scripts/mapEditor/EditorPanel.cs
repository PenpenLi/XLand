using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class EditorPanel : MonoBehaviour {

	private const float gap = 0.1f;

	private float s3 = Mathf.Sqrt (3);

	private List<GameObject> gos = new List<GameObject> ();

	private short width;

	private short height;

	GameObject container;

	GameObject resource;

	void Awake(){

		container = new GameObject ("Container");
		
		resource = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/SixPanel.prefab");
	}

	public void Init(string _path){

		BinaryReader reader = new BinaryReader(new FileStream (_path,FileMode.Open));

		width = reader.ReadInt16 ();

		height = reader.ReadInt16 ();

		for (int i = 0; i < height; i++) {
			
			if(i % 2 == 0){
				
				for(int m = 0 ; m < width ; m++){
					
					GameObject go = GameObject.Instantiate<GameObject>(resource);
					
					go.transform.SetParent(container.transform,false);
					
					Cell cell = go.AddComponent<Cell>();
					
					cell.index = gos.Count;

					cell.type = reader.ReadInt16 ();
					
					gos.Add(go);
					
					go.transform.localPosition = new Vector3(m * (s3 + gap),0,-i * (s3 + gap) / 2 * s3);
				}
				
			}else{
				
				for(int m = 0 ; m < width - 1 ; m++){
					
					GameObject go = GameObject.Instantiate<GameObject>(resource);
					
					go.transform.SetParent(container.transform,false);
					
					Cell cell = go.AddComponent<Cell>();
					
					cell.index = gos.Count;

					cell.type = reader.ReadInt16 ();
					
					gos.Add(go);
					
					go.transform.localPosition = new Vector3(m * (s3 + gap) + (s3 + gap) / 2,0,-i * (s3 + gap) / 2 * s3);
				}
			}
		}
		
		container.transform.localPosition = gos [(int)(gos.Count / 2)].transform.position * -1;

		reader.Close ();
	}

	public void Init(short _width,short _height){

		width = _width;

		height = _height;

		for (int i = 0; i < height; i++) {

			if(i % 2 == 0){

				for(int m = 0 ; m < width ; m++){

					GameObject go = GameObject.Instantiate<GameObject>(resource);
					
					go.transform.SetParent(container.transform,false);

					Cell cell = go.AddComponent<Cell>();

					cell.index = gos.Count;
					
					gos.Add(go);

					go.transform.localPosition = new Vector3(m * (s3 + gap),0,-i * (s3 + gap) / 2 * s3);
				}

			}else{

				for(int m = 0 ; m < width - 1 ; m++){

					GameObject go = GameObject.Instantiate<GameObject>(resource);
					
					go.transform.SetParent(container.transform,false);

					Cell cell = go.AddComponent<Cell>();
					
					cell.index = gos.Count;

					gos.Add(go);
					
					go.transform.localPosition = new Vector3(m * (s3 + gap) + (s3 + gap) / 2,0,-i * (s3 + gap) / 2 * s3);
				}
			}
		}

		container.transform.localPosition = gos [(int)(gos.Count / 2)].transform.position * -1;
	}

	public void BtClick(){

		string path = EditorUtility.SaveFilePanel ("Save file", "", "", "map");

		if (string.IsNullOrEmpty (path)) {

			return;
		}

		if (File.Exists (path)) {

			File.Delete(path);
		}

		BinaryWriter writer = new BinaryWriter(new FileStream (path,FileMode.CreateNew));

		writer.Write (width);

		writer.Write (height);

		for (int i = 0; i < gos.Count; i++) {

			writer.Write(gos[i].GetComponent<Cell>().type);
		}

		writer.Close ();
	}
}
