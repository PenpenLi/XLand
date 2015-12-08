using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace xy3d.tstd.lib.systemIO{

	public class DeserializeInThread{

		private static DeserializeInThread _Instance;

		public static DeserializeInThread Instance{

			get{

				if(_Instance == null){

					_Instance = new DeserializeInThread();
				}

				return _Instance;
			}
		}

		private DeserializeInThreadScript script;

		public DeserializeInThread(){

			GameObject go = new GameObject();

			go.name = "DeserializeInThreadGameObject";

			GameObject.DontDestroyOnLoad(go);

			script = go.AddComponent<DeserializeInThreadScript>();
		}

		public void Deserialize<T>(byte[] _bytes,Action<T> _callBack){

			script.Deserialize(_bytes,_callBack);
		}

	}
}
