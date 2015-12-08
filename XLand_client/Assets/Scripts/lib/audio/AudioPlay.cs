using UnityEngine;
using System.Collections;
using System;
using xy3d.tstd.lib.assetManager;

namespace xy3d.tstd.lib.audio{

	public class AudioPlay{

		private static AudioPlay _Instance;

		public static AudioPlay Instance{

			get{

				if(_Instance == null){

					_Instance = new AudioPlay();
				}

				return _Instance;
			}
		}

		private AudioPlayScript script;

		private AudioSource musicSource;

		public bool isMusicPlay = true;

		public bool isEffectPlay = true;

		public AudioPlay(){

			GameObject musicPlayer = new GameObject("MusicPlayer");

			musicSource = musicPlayer.AddComponent<AudioSource>();

			musicSource.loop = true;

			script = musicPlayer.AddComponent<AudioPlayScript>();
		}

		public void PlayMusic(string _path){

			AudioFactory.Instance.GetClip(_path,GetMusicClip);
		}

		private void GetMusicClip(AudioClip _clip){

			musicSource.clip = _clip;

			if(isMusicPlay){

				musicSource.Play();
			}
		}

		public void PlayEffect(string _path){

			if(isEffectPlay){

				AudioFactory.Instance.GetClip(_path,GetEffectClip);
			}
		}

		private void GetEffectClip(AudioClip _clip){

			script.PlayEffect(_clip);
		}

		public void SetIsMusicPlay(bool _b){

			isMusicPlay = _b;

			if(!isMusicPlay){

				if(musicSource.isPlaying){

					musicSource.Stop();
				}

			}else{

				if(musicSource.clip != null){

					musicSource.Play();
				}
			}
		}

		public void SetIsEffectPlay(bool _b){

			isEffectPlay = _b;
		}
	}
}