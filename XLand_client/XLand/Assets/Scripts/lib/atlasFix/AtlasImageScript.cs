using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AtlasImageScript : MonoBehaviour {

	public AtlasData atlasData;
	public string spriteName;

	// Use this for initialization
	void Start () {
	
		Image img = GetComponent<Image>();

		img.sprite = atlasData.sprites[Array.IndexOf(atlasData.names,spriteName)];

		GameObject.Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
