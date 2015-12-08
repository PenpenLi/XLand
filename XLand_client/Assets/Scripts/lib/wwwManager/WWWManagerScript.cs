using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.wwwManager
{

	public class WWWManagerScript : MonoBehaviour
	{

#if PLATFORM_PC

		private static string path = "file:///" + Application.streamingAssetsPath + "/";

#endif

#if PLATFORM_ANDROID

		private static string path = Application.streamingAssetsPath + "/";

#endif

#if PLATFORM_IOS

		private static string path = Application.streamingAssetsPath + "/";

#endif

		private List<Action> callBackList = new List<Action> ();

		WWWManager.fixUrlDelegate fixUrlDelegate;

		public void SetUrlFixFun (WWWManager.fixUrlDelegate _callBack)
		{
			
			fixUrlDelegate = _callBack;
		}

		public void Load (string _path, bool _isRemote, Action<WWW> _callBack)
		{
//			Action ddd = delegate() {
//
//				StartCoroutine (LoadCorotine (_path, _isRemote, _callBack));
//			};
//
//			SuperTween.Instance.DelayCall(4,ddd);

			StartCoroutine (LoadCorotine (_path, _isRemote, _callBack));
		}

		private IEnumerator LoadCorotine (string _path, bool _isRemote, Action<WWW> _callBack)
		{

			string finalPath;

			if (!_isRemote) {

				if (fixUrlDelegate == null) {

					finalPath = path + _path;

				} else {

					bool b = fixUrlDelegate (ref _path);

					if (b) {

						finalPath = _path;

					} else {

						finalPath = path + _path;
					}
				}

			} else {

				finalPath = _path;
			}

			WWW www = new WWW (finalPath);

			yield return www;

//			SuperDebug.Log ("资源加载成功:" + _path);

			if (www.error != null) {

				SuperDebug.Log ("WWW download had an error:" + www.error + "  finalPath:" + finalPath);

//				throw new Exception ("WWW download had an error:" + www.error);
			}

			Action callBack = delegate() {

				_callBack (www);

				www.Dispose ();
			};

			callBackList.Add (callBack);
		}

		void Update ()
		{

			if (callBackList.Count > 0) {

				foreach (Action callBack in callBackList) {

					callBack ();
				}

				callBackList.Clear ();
			}
		}
	}
}