using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayScript : MonoBehaviour {

	private List<AudioSource> effectSources = new List<AudioSource>();

	public void PlayEffect(AudioClip _clip){

		AudioSource source = gameObject.AddComponent<AudioSource>();

		source.clip = _clip;

		source.Play();

		effectSources.Add(source);
	}

	// Update is called once per frame
	void Update () {
	
		for(int i = effectSources.Count - 1 ; i > -1 ; i--){

			if(!effectSources[i].isPlaying){

				GameObject.Destroy(effectSources[i]);

				effectSources.RemoveAt(i);
			}
		}
	}
}
