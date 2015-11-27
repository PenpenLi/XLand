using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XLand_proto;
using SuperProto;

public class s : MonoBehaviour {

	InputField inputName;
	InputField inputPwd;

	void Start(){

		inputName = GameObject.Find ("InputName").GetComponent<InputField> ();
		inputPwd = GameObject.Find ("InputPwd").GetComponent<InputField> ();

		SuperSocket.Instance.Connect ("127.0.0.1", 1983, ConnectOK);
	}

	private void ConnectOK(){

		SuperDebug.Log ("ConnectOK");
	}

	public void Click(){

		LoginProto login = new LoginProto ();

		login.userName = inputName.text;

		login.password = inputPwd.text;

		SuperSocket.Instance.SendData<LoginResultProto> (login, LoginOver);
	}

	private void LoginOver(LoginResultProto _result){

		SuperDebug.Log ("result:" + _result.result);
	}

	void Update(){

		if(Input.GetKeyUp(KeyCode.A)){

			UserDataProto ll = new UserDataProto();

			SuperSocket.Instance.SendData<UserDataResultProto>(ll,GetData);
		}
	}

	private void GetData(UserDataResultProto dd){

		SuperDebug.Log ("GetData:" + dd);
	}
}
