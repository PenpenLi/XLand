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

		private bool isDrag ;
		
		public override void OnBeginDrag(PointerEventData eventData) {

			if(!canDrag){

				return;
			}

			if(isRestrain){

				isDrag = true;
			}
			
			base.OnBeginDrag(eventData);
		}
		
		public override void OnDrag(PointerEventData eventData) {

			if(!canDrag){
				
				return;
			}

			if(!isRestrain || isDrag){

				base.OnDrag(eventData);
			}
		}
		
		public void OnPointerExit (PointerEventData eventData)
		{
			if(isRestrain){

				base.OnEndDrag(eventData);
				
				isDrag = false;
			}
		}
	}
}