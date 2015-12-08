using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.audio{

	public class AudioFactory{

		private static AudioFactory _Instance;

		public static AudioFactory Instance{

			get{

				if(_Instance == null){

					_Instance = new AudioFactory();
				}

				return _Instance;
			}
		}

		private Dictionary<string,AudioFactoryUnit> dic = new Dictionary<string, AudioFactoryUnit>();

		public AudioClip GetClip(string _name,Action<AudioClip> _callBack){

			return GetClip(_name,_callBack,true);
		}

		public AudioClip GetClip(string _name,Action<AudioClip> _callBack,bool _willDispose){

			AudioFactoryUnit unit;
			
			if (!dic.ContainsKey (_name)) {
				
				unit = new AudioFactoryUnit (_name);
				
				dic.Add (_name, unit);
				
			} else {
				
				unit = dic [_name];
			}
			
			return unit.GetClip(_callBack,_willDispose);
		}

		public void Dispose(){

			Dictionary<string,AudioFactoryUnit> tmpDic = new Dictionary<string, AudioFactoryUnit>();

			foreach(KeyValuePair<String,AudioFactoryUnit> pair in dic){

				AudioFactoryUnit unit = pair.Value;

				if(!unit.willDispose){

					tmpDic.Add(pair.Key,unit);
				}
			}

			dic = tmpDic;
		}
	}
}