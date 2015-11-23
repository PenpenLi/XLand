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

	private static readonly int BODY_LENGTH = 1024;

	private byte[] headBuffer = new byte[HEAD_LENGTH];

	private int headLength;

	private int headOffset;

	private byte[] bodyBuffer = new byte[BODY_LENGTH];

	private int bodyLength;

	private int bodyOffset;

	private List<BaseProto> receivePool;

	private MemoryStream receiveStream = new MemoryStream();
	
	private BinaryFormatter reveiveFormatter = new BinaryFormatter();
	
	private MemoryStream sendStream = new MemoryStream();
	
	private BinaryFormatter sendFormatter = new BinaryFormatter();

	private bool isWaittingForResponse;

	private List<BaseProto> sendPool = new List<BaseProto>();

	public void Connect(string _ip,int _port,Action _callBack,List<BaseProto> _pool){

		receivePool = _pool;

		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		IPEndPoint ipe = new IPEndPoint (IPAddress.Parse (_ip), _port);

		socket.BeginConnect (ipe, ConnectEnd, null);
	}

	private void ConnectEnd(IAsyncResult _result){

		socket.EndConnect (_result);

		headLength = HEAD_LENGTH;
		
		headOffset = 0;

		ReceiveHead ();
	}

	private void ReceiveHead(){

		socket.BeginReceive (headBuffer, headOffset, headLength, SocketFlags.None, ReceiveHeadEnd, null);
	}

	private void ReceiveHeadEnd(IAsyncResult _result){

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

		socket.BeginReceive(bodyBuffer, bodyOffset, bodyLength, SocketFlags.None, ReceiveBodyEnd, null);
	}

	private void ReceiveBodyEnd(IAsyncResult _result){

		int length = socket.EndReceive (_result);
		
		if (length == 0) {
			
			SuperDebug.LogError ("Disconnect!!!");
			
		} else if (length < headLength) {
			
			bodyLength = bodyLength - length;
			
			bodyOffset = bodyOffset + length;
			
			ReceiveBody();
			
		} else {
			
			receiveStream.Position = 0;
			
			receiveStream.Write(bodyBuffer, 0, bodyOffset + length);
			
			receiveStream.Position = 0;
			
			BaseProto data = reveiveFormatter.Deserialize(receiveStream) as BaseProto;

			switch(data.type){

			case PROTO_TYPE.C2S:

				SuperDebug.LogError("error1!");

				return;

			case PROTO_TYPE.S2C:

				BaseProto sendData = null;
				
				lock(sendPool){

					if(!isWaittingForResponse){

						SuperDebug.LogError("error6!");

						return;
					}
					
					if(sendPool.Count > 0){
						
						sendData = sendPool[0];
						
						sendPool.RemoveAt(0);
						
					}else{
						
						isWaittingForResponse = false;
					}
				}
				
				if (sendData != null) {
					
					SendDataReal (sendData);
				}

				break;
			}

			lock(receivePool){

				receivePool.Add(data);
			}

			ReceiveHead ();
		}
	}

	public void SendData(BaseProto _data){

		if (_data.type != PROTO_TYPE.C2S) {

			SuperDebug.LogError ("sendError!");

			return;
		}

		lock (sendPool) {

			if (isWaittingForResponse) {

				sendPool.Add (_data);

				return;

			} else {

				isWaittingForResponse = true;
			}
		}

		SendDataReal (_data);
	}

	private void SendDataReal(BaseProto _data){

		sendFormatter.Serialize(sendStream, _data);

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
