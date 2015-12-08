using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace xy3d.tstd.lib.superGraphicRaycast{

	public class SuperGraphicRaycast : GraphicRaycaster {

		private static int m_isOpen = 1;

		public static bool isOpen{

			get{

				return m_isOpen > 0;
			}
		}

		public static void SetOpen(bool _b){

            SuperDebug.Log("SetOpen: " + _b);

			m_isOpen = m_isOpen + (_b ? 1 : -1);

			if(m_isOpen > 1){

				SuperDebug.LogError("SuperGraphicRaycast.SetOpen error!");
			}
		}

		public static bool filter = false;

		public static string filterTag;

		private int touchCount = 0;

		void LateUpdate(){

			if(touchCount != 0){

				touchCount = 0;
			}
		}

		public override void Raycast (PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if(!isOpen){

				return;
			}

			if(touchCount > 0){
				
				return;
			}
			
			touchCount++;

//			if(Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)){

				base.Raycast(eventData,resultAppendList);
				
				if(filter){
					
					for(int i = resultAppendList.Count - 1 ; i > -1 ; i--){
						
						if(!resultAppendList[i].gameObject.CompareTag(filterTag)){
							
							resultAppendList.RemoveAt(i);
						}
					}
				}
//			}
		}
	}
}