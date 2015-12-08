using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SuperProto;

public class PlayerData {

	private static PlayerData _Instance;

	public static PlayerData Instance{

		get{

			if(_Instance == null){

				_Instance = new PlayerData();
			}

			return _Instance;
		}
	}

	public int money;

	public Dictionary<int, int> cards;
}
