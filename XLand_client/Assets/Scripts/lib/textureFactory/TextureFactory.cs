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

		public bool SetSprite(Image _img,string _name,bool _doNotDispose){

			ImageScript script = null;

			if(!_doNotDispose){

				script = _img.GetComponent<ImageScript>();

				if(script == null){

					script = _img.gameObject.AddComponent<ImageScript>();

					script.reloadCallBack = SetSpriteReal;
				}

				script.isLoading = true;

				script.imgName = _name;
			}

			return SetSpriteReal(_img,script,_name,_doNotDispose);
		}

		private bool SetSpriteReal(Image _img,ImageScript _script,string _name,bool _doNotDispose){

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

				GetSprite(_img,_script,_name,_doNotDispose,obj);
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

				return true;

			}else{

				_img.color = Color.white;

				return false;
			}
		}

		private void GetSprite(Image _img,ImageScript _script,string _name,bool _doNotDispose,Sprite _sp){

			if(!_doNotDispose){

				if(_script.imgName != _name){

					return;
				}

				_script.isLoading = false;
			}

			_img.sprite = _sp;
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

			foreach (ITextureFactoryUnit unit in dicWillDispose.Values) {

				unit.Dispose ();
			}

			dicWillDispose.Clear ();
		}
	}
}
