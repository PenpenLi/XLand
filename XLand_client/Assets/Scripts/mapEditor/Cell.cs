using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public int index;

	public short type = 0;

	private Material material;

	// Use this for initialization
	void Start () {
	
		material = GetComponent<Renderer> ().material;

		if (type == 0) {

			material.SetColor ("_Color", Color.white);
			
		} else if (type == 1) {

			material.SetColor ("_Color", Color.green);
			
		}else{

			material.SetColor ("_Color", Color.red);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown(){

		if (type == 0) {

			type = 1;

			material.SetColor ("_Color", Color.green);

		} else if (type == 1) {

			type = 2;
			
			material.SetColor ("_Color", Color.red);

		}else{
			
			type = 0;
			
			material.SetColor ("_Color", Color.white);
		}
	}
}
