using UnityEngine;
using System.Collections;

public class AssemblyShell : MonoBehaviour {

	public static readonly string ASSEMBLY_FILE_NAME = "Script.bytes";

	// Use this for initialization
	void Start () {

//		Application.targetFrameRate = 30;

//		Input.multiTouchEnabled = false;

#if PLATFORM_PC

		AssemblyManager.Instance.Init("file:///" + Application.persistentDataPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyUpdated);

#else

		AssemblyManager.Instance.Init("file:///" + Application.persistentDataPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyUpdated);
#endif
	}

	private void GetAssemblyUpdated(bool _result){
		
		if(_result){

			SuperDebug.Log("找到了新的代码文件！！！");

			Application.LoadLevel("main");
			
		}else{

#if PLATFORM_PC

			AssemblyManager.Instance.Init("file:///" + Application.streamingAssetsPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyOriginal);

#else
			AssemblyManager.Instance.Init(Application.streamingAssetsPath + "/" + ASSEMBLY_FILE_NAME,GetAssemblyOriginal);

#endif

		}
	}
	
	private void GetAssemblyOriginal(bool _result){

		SuperDebug.Log("没有找到新的代码文件！！！" + _result);
		
		Application.LoadLevel("main");
	}
}
