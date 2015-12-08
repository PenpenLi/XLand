using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

public class TitlePanel : MonoBehaviour {
	
	private InputField widthField;
	private InputField heightField;
	
	// Use this for initialization
	void Start () {
		
		widthField = GameObject.Find ("widthField").GetComponent<InputField> ();
		
		widthField.contentType = InputField.ContentType.DecimalNumber;
		
		heightField = GameObject.Find ("heightField").GetComponent<InputField> ();
		
		heightField.contentType = InputField.ContentType.DecimalNumber;
	}
	
	public void CreateBtClick(){
		
		if (!string.IsNullOrEmpty (widthField.text) && !string.IsNullOrEmpty (heightField.text)) {

			MapEditor editor = GetComponentInParent<MapEditor>();

			editor.SwitchToEditorPanel(short.Parse(widthField.text),short.Parse(heightField.text));
		}
	}

	public void OpenBtClick(){
		
		string path = EditorUtility.OpenFilePanel ("Open File", "", "map");

		if (!string.IsNullOrEmpty (path)) {

			MapEditor editor = GetComponentInParent<MapEditor> ();
			
			editor.SwitchToEditorPanel (path);
		}
	}
}
