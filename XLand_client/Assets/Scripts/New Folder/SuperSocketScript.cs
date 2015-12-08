using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SuperSocketScript : MonoBehaviour {

	private List<Action> receivePool;

	public void Init(List<Action> _receivePool){

		receivePool = _receivePool;
	}
	
	// Update is called once per frame
	void Update () {
	
		while (true) {

			Action callBack = null;

			lock(receivePool){

				if(receivePool.Count == 0){

					return;

				}else{

					callBack = receivePool[0];

					receivePool.RemoveAt(0);
				}
			}

			if(callBack != null){

				callBack();
			}
		}
	}
}
