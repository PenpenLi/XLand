using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using SuperProto;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SuperSocket {

	private static SuperSocket _Instance;

	public static SuperSocket Instance{

		get{

			if(_Instance == null){

				_Instance = new SuperSocket();
			}

			return _Instance;
		}
	}

	private Socket socket;

	private static readonly int HEAD_LENGTH = 4;

	private static readonly int BODY_LENGTH = 10240;

	private byte[] headBuffer = new byte[HEAD_LENGTH];

	private int headLength;

	private int headOffset;

	private byte[] bodyBuffer = new byte[BODY_LENGTH];

	private int bodyLength;

	private int bodyOffset;

	private List<Action> receivePool = new List<Action>();

	private MemoryStream receiveStream = new MemoryStream();
	
	private BinaryFormatter reveiveFormatter = new BinaryFormatter();
	
	private MemoryStream sendStream = new MemoryStream();
	
	private BinaryFormatter sendFormatter = new BinaryFormatter();

	private bool isWaittingForResponse;

	private List<BaseProto> sendPool = new List<BaseProto>();

	private List<Action<BaseProto>> callBackPool = new List<Action<BaseProto>> ();

	private Action<BaseProto> nowCallBack;

	private Dictionary<Type,Action<BaseProto>> pushDic = new Dictionary<Type, Action<BaseProto>> ();

	public SuperSocket(){

		GameObject go = new GameObject ("SuperSocketGameObject");

		SuperSocketScript script = go.AddComponent<SuperSocketScript> ();

		script.Init (receivePool);

		GameObject.DontDestroyOnLoad(go);
	}

	public void Connect(string _ip,int _port,Action _callBack){

		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		IPEndPoint ipe = new IPEndPoint (IPAddress.Parse (_ip), _port);

		socket.BeginConnect (ipe, ConnectEnd, _callBack);
	}

	public void RegisterPushDataCallBack<T>(Action<T> _callBack) where T : BaseProto{

		Action<BaseProto> tmpCallBack = delegate(BaseProto obj) {

			_callBack (obj as T);
		};

		pushDic.Add (typeof(T), tmpCallBack);
	}

	private void ConnectEnd(IAsyncResult _result){

		socket.EndConnect (_result);

		Action callBack = _result.AsyncState as Action;

		callBack ();

		headLength = HEAD_LENGTH;
		
		headOffset = 0;

		ReceiveHead ();
	}

	private void ReceiveHead(){

		Debug.Log ("ReceiveHead!");

		socket.BeginReceive (headBuffer, headOffset, headLength, SocketFlags.None, ReceiveHeadEnd, null);
	}

	private void ReceiveHeadEnd(IAsyncResult _result){

		Debug.Log ("ReceiveHeadEnd!");

		int length = socket.EndReceive (_result);

		if (length == 0) {

			SuperDebug.LogError ("Disconnect!!!");

		} else if (length < headLength) {

			headLength = headLength - length;

			headOffset = headOffset + length;

			ReceiveHead();

		} else {

			bodyLength = BitConverter.ToInt32(headBuffer, 0);
			
			bodyOffset = 0;

			headLength = HEAD_LENGTH;
			
			headOffset = 0;

			ReceiveBody();
		}
	}

	private void ReceiveBody(){

		Debug.Log ("ReceiveBody!" + bodyLength);

		socket.BeginReceive(bodyBuffer, bodyOffset, bodyLength, SocketFlags.None, ReceiveBodyEnd, null);
	}

	private void ReceiveBodyEnd(IAsyncResult _result){

		int length = socket.EndReceive (_result);

		Debug.Log ("ReceiveBodyEnd!" + length + "   " + bodyLength);
		
		if (length == 0) {
			
			SuperDebug.LogError ("Disconnect!!!");
			
		} else if (length < bodyLength) {
			
			bodyLength = bodyLength - length;
			
			bodyOffset = bodyOffset + length;
			
			ReceiveBody();
			
		} else {

			Debug.Log ("all!" + length + "   " + bodyLength);
			
			receiveStream.Position = 0;
			
			receiveStream.Write(bodyBuffer, 0, bodyOffset + length);
			
			receiveStream.Position = 0;

			BaseProto data = null;

			try{

				data = reveiveFormatter.Deserialize(receiveStream) as BaseProto;

			}catch(Exception e){


				Debug.Log(e.ToString());
			}

			Debug.Log("receive:" + data.GetType().ToString());

			switch(data.type){

			case PROTO_TYPE.C2S:

				SuperDebug.LogError("error1!");

				return;

			case PROTO_TYPE.S2C:



				BaseProto sendData = null;

				Action<BaseProto> tmpCallBack = nowCallBack;
				
				nowCallBack = null;
				
				Action callBack = delegate() {
					
					tmpCallBack(data);
				};
				
				lock(receivePool){
					
					receivePool.Add(callBack);
				}

				lock(sendPool){

					if(!isWaittingForResponse){

						SuperDebug.LogError("error6!");

						return;
					}
					
					if(sendPool.Count > 0){
						
						sendData = sendPool[0];
						
						sendPool.RemoveAt(0);

						nowCallBack = callBackPool[0];

						callBackPool.RemoveAt(0);
						
					}else{
						
						isWaittingForResponse = false;
					}
				}

				if (sendData != null) {
					
					SendDataReal (sendData);
				}

				break;

			default:

				Type protoType = data.GetType();

				if(pushDic.ContainsKey(protoType)){

					callBack = delegate() {

						pushDic[protoType](data);
					};

					lock(receivePool){
						
						receivePool.Add(callBack);
					}

				}else{

					SuperDebug.LogError("error669!");
				}

				break;
			}

			ReceiveHead ();
		}
	}

	public void SendData<T>(BaseProto _data,Action<T> _callBack) where T : BaseProto{

		if (_data.type != PROTO_TYPE.C2S) {

			SuperDebug.LogError ("sendError!");

			return;
		}

		Action<BaseProto> callBack = delegate(BaseProto obj) {

			_callBack(obj as T);
		};

		lock (sendPool) {

			if (isWaittingForResponse) {

				sendPool.Add (_data);

				callBackPool.Add(callBack);

				return;

			} else {

				isWaittingForResponse = true;

				nowCallBack = callBack;
			}
		}

		SendDataReal (_data);
	}

	private void SendDataReal(BaseProto _data){

		sendFormatter.Serialize(sendStream, _data);
		
		sendStream.Position = 0;

		socket.BeginSend(BitConverter.GetBytes(sendStream.GetBuffer().Length), 0, 4, SocketFlags.None, SendHeadEnd, null);
	}

	private void SendHeadEnd(IAsyncResult _result){

		socket.EndSend (_result);

		socket.BeginSend (sendStream.GetBuffer (), 0, sendStream.GetBuffer ().Length, SocketFlags.None, SendBodyEnd, null);
	}

	private void SendBodyEnd(IAsyncResult _result){

		socket.EndSend (_result);
	}
}
