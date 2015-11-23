using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading;

namespace xy3d.tstd.lib.systemIO{

	public class DeserializeInThreadScript : MonoBehaviour {

		private List<IDeserializeInThreadUnit> list = new List<IDeserializeInThreadUnit>();

		public void Deserialize<T>(byte[] _bytes,Action<T> _callBack){
			
			DeserializeInThreadUnit<T> data = new DeserializeInThreadUnit<T>();
			
			data.callBack = _callBack;
			data.bytes = _bytes;
			
			list.Add(data);
			
			Thread thread = new Thread(data.Start);
			
			thread.Start();
		}

		// Update is called once per frame
		void Update () {

			for(int i = list.Count - 1 ; i > -1 ; i--){

				bool result = list[i].CallBack();
				
				if(result){
					
					list.RemoveAt(i);
				}
			}
		}
	}
}