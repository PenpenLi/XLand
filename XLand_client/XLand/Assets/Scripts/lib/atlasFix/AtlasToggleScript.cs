using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AtlasToggleScript : MonoBehaviour {
	
	public AtlasData disabledSpriteAtlasData;
	public AtlasData highlightedSpriteAtlasData;
	public AtlasData pressedSpriteAtlasData;

	public string disabledSpriteName;
	public string highlightedSpriteName;
	public string pressedSpriteName;
	
	// Use this for initialization
	void Start () {
		
		Toggle toggle = GetComponent<Toggle>();

		SpriteState spriteState = new SpriteState();

		if(!string.IsNullOrEmpty(disabledSpriteName)){

			spriteState.disabledSprite = disabledSpriteAtlasData.sprites[Array.IndexOf(disabledSpriteAtlasData.names,disabledSpriteName)];
		}

		if(!string.IsNullOrEmpty(highlightedSpriteName)){
			
			spriteState.highlightedSprite = highlightedSpriteAtlasData.sprites[Array.IndexOf(highlightedSpriteAtlasData.names,highlightedSpriteName)];
		}

		if(!string.IsNullOrEmpty(pressedSpriteName)){
			
			spriteState.pressedSprite = pressedSpriteAtlasData.sprites[Array.IndexOf(pressedSpriteAtlasData.names,pressedSpriteName)];
		}

		toggle.spriteState = spriteState;
		
		GameObject.Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
