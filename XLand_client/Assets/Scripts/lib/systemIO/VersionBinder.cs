using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;
using System.Reflection;

namespace xy3d.tstd.lib.systemIO{

	public class VersionBinder : SerializationBinder {

		public override Type BindToType (string assemblyName, string typeName){

			String assemVer1 = Assembly.GetExecutingAssembly().FullName;

			string resultTypeName = String.Format("{0}, {1}", typeName, assemVer1);

			return Type.GetType(resultTypeName);

	//		return Type.GetType(typeName + assemVer1);
		}
	}
}