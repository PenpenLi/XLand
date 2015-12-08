using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using xy3d.tstd.lib.superTween;
using System;

namespace xy3d.tstd.lib.superList{

	public class SuperListCell : MonoBehaviour,IPointerClickHandler {

		protected object data;
		protected bool selected;

		public CanvasGroup canvasGroup;

		[HideInInspector]public int index;

		public void OnPointerClick (PointerEventData eventData)
		{
			SendMessageUpwards("CellClick",this);
		}  
		
		// Update is called once per frame
		void Update () {
		
		}

		public virtual bool SetData(object _data){

			data = _data;

			return false;
		}

		public virtual void SetSelected(bool _value){

			selected = _value;
		}

		public bool GetSelected()
		{
			return selected;
		}

		private int tweenID;

		public void AlphaIn(Action _callBack){

			tweenID = SuperTween.Instance.To(0,1,0.1f,AlphaInDel,_callBack);
		}

		public void StopAlphaIn(){

			SuperTween.Instance.Remove(tweenID);
		}

		private void AlphaInDel(float _value){

			canvasGroup.alpha = _value;
		}
	}
}
