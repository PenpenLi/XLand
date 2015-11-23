using UnityEngine;
using System.Collections;

public class s : MonoBehaviour {

	public Color color;

	private Material material;

	// Use this for initialization
	void Start () {

		color = Color.white;
	
		material = GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
	
		material.SetColor ("_Color", color);
	}

	void OnMouseOver(){


		Debug.Log ("over!");
	}
}
