using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class MapEditor : MonoBehaviour {

	private GameObject titlePanel;
	private GameObject editorPanel;

	// Use this for initialization
	void Start () {
	
		titlePanel = GameObject.Find ("TitlePanel");

		editorPanel = GameObject.Find ("EditorPanel");

		editorPanel.SetActive (false);

//		GameObject resource = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/SixPanel.prefab");
//
//		GameObject go = GameObject.Instantiate<GameObject> (resource);
//
//		go.transform.localPosition = new Vector3 (0, 1, 0);
//
//		GameObject go2 = GameObject.Instantiate<GameObject> (resource);
//
//		go2.GetComponent<Renderer> ().material.SetColor ("_Color", Color.red);
	}
	
	public void SwitchToEditorPanel(short _width,short _height){

		titlePanel.SetActive (false);

		editorPanel.SetActive (true);

		editorPanel.GetComponent<EditorPanel> ().Init (_width, _height);
	}

	public void SwitchToEditorPanel(string _path){

		titlePanel.SetActive (false);
		
		editorPanel.SetActive (true);
		
		editorPanel.GetComponent<EditorPanel> ().Init (_path);
	}
}
