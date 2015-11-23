using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using xy3d.tstd.lib.superTween;

namespace xy3d.tstd.lib.textureFactory{

	public class TextureFactory{

		private static TextureFactory _Instance;
		
		public static TextureFactory Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new TextureFactory ();
				}
				
				return _Instance;
			}
		}

		public Dictionary<string,ITextureFactoryUnit> dic  = new Dictionary<string, ITextureFactoryUnit>();
		public Dictionary<string,ITextureFactoryUnit> dicWillDispose  = new Dictionary<string, ITextureFactoryUnit>();

		public void SetSprite(Image _img,string _name,bool _doNotDispose){

			TextureFactoryUnit<Sprite> unit;

			Dictionary<string,ITextureFactoryUnit> tmpDic;

			if(_doNotDispose){

				tmpDic = dic;

			}else{

				tmpDic = dicWillDispose;
			}
			
			if (!tmpDic.ContainsKey (_name)) {
				
				unit = new TextureFactoryUnit<Sprite> (_name);
				
				tmpDic.Add (_name, unit);
				
			} else {
				
				unit = tmpDic [_name] as TextureFactoryUnit<Sprite>;
			}

			Action<Sprite> callBack = delegate(Sprite obj) {

				GetSprite(_img,_name,_doNotDispose,obj);
			};

			Sprite result = unit.GetTexture(callBack);

			if(result == null){

				_img.color = new Color(1,1,1,0);

				Action<float> tween = delegate(float obj) {
					
					ChangeImageAlpha(_img,obj);
				};

				Action<Sprite> startTween = delegate(Sprite obj) {

					SuperTween.Instance.To(0,1,0.1f,tween,null);
				};
				
				unit.GetTexture(startTween);
			}
		}

		private void GetSprite(Image _img,string _name,bool _doNotDispose,Sprite _sp){

			_img.sprite = _sp;

			if(!_doNotDispose){

				ImageScript imageScript = _img.GetComponent<ImageScript>();

				if(imageScript == null){

					imageScript = _img.gameObject.AddComponent<ImageScript>();
				}

				imageScript.imgName = _name;
			}

//			if(_img.color.a == 0){
//
//				Action<float> callBack = delegate(float obj) {
//
//					ChangeImageAlpha(_img,obj);
//				};
//
//				SuperTween.Instance.To(0,1,0.1f,callBack,null);
//			}
		}

		private void ChangeImageAlpha(Image _img,float _alpha){

			_img.color = new Color(1,1,1,_alpha);
		}

		public T GetTexture<T> (string _name,Action<T> _callBack,bool _doNotDispose) where T:UnityEngine.Object {
			
			TextureFactoryUnit<T> unit;
			
			Dictionary<string,ITextureFactoryUnit> tmpDic;
			
			if (_doNotDispose) {
				
				tmpDic = dic;
				
			} else {
				
				tmpDic = dicWillDispose;
			}
			
			if (!tmpDic.ContainsKey (_name)) {
				
				unit = new TextureFactoryUnit<T> (_name);
				
				tmpDic.Add (_name, unit);
				
			} else {
				
				unit = tmpDic [_name] as TextureFactoryUnit<T>;
			}
			
			return unit.GetTexture(_callBack);
		}

		public void Dispose(){

			foreach (KeyValuePair<string,ITextureFactoryUnit> pair in dicWillDispose) {

				pair.Value.Dispose ();
			}

			dicWillDispose.Clear ();
		}
	}
}
