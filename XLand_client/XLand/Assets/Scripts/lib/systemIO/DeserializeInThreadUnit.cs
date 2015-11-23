using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace xy3d.tstd.lib.systemIO{

	public class DeserializeInThreadUnit<T> : IDeserializeInThreadUnit {

		public Action<T> callBack;
		public byte[] bytes;
		private T result;
		private bool isOver;

		public void Start(){

			using (Stream stream = new MemoryStream(bytes))
			{
				
				BinaryFormatter formatter = new BinaryFormatter();
				
				formatter.Binder = new VersionBinder();
				
				result = (T) formatter.Deserialize(stream);
				
				isOver = true;
			}
		}

		public bool CallBack(){

			if(isOver){

				callBack(result);
			}

			return isOver;
		}
	}
}