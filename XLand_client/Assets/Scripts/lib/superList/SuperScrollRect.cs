using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace xy3d.tstd.lib.superList{
	
	public class SuperScrollRect : ScrollRect,IPointerExitHandler {

		private static int m_canDrag = 1;

		public static void SetCanDrag(bool _b){

			m_canDrag = m_canDrag + (_b ? 1 : -1);
		}

		public static bool canDrag{

			get{

				return m_canDrag > 0;
			}
		}

		public bool isRestrain = false;

		private bool isRestrainDrag;

		private bool isOneTouchDrag;
		
		public override void OnBeginDrag(PointerEventData eventData) {

			if(!canDrag || Input.touchCount > 1){

				return;
			}

			if(isRestrain){

				isRestrainDrag = true;
			}

			isOneTouchDrag = true;

			base.OnBeginDrag(eventData);
		}
		
		public override void OnDrag(PointerEventData eventData) {

			if(!canDrag){
				
				return;
			}
			
			if(Input.touchCount > 1){
				
				isOneTouchDrag = false;
				
				return;
			}

			if(isOneTouchDrag && (!isRestrain || isRestrainDrag)){

				base.OnDrag(eventData);
			}
		}
		
		public void OnPointerExit (PointerEventData eventData)
		{
			if(isRestrain){

				base.OnEndDrag(eventData);
				
				isRestrainDrag = false;
			}
		}
	}
}