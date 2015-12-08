using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.textureFactory;
using xy3d.tstd.lib.superTween;
using System;

namespace xy3d.tstd.lib.textureFactory{

	public class ImageScript : MonoBehaviour {
		
		private Image img;
		
		public string imgName;

		public bool isLoading;

		public Func<Image,ImageScript,string,bool,bool> reloadCallBack;

		void Awake(){

			img = GetComponent<Image>();
		}

		void OnEnable(){

			if(img.sprite == null){

				//因为有可能image已经被刷新了  所以这个自动恢复隔一帧进行
				img.color = new Color(1,1,1,0);

				SuperTween.Instance.DelayCall(0,Reload);
			}
		}

		private void Reload(){

			if(!isLoading && img.sprite == null){

				reloadCallBack(img,this,imgName,false);
			}
		}
	}
}