using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;
using System;
using System.Collections.Generic;

namespace xy3d.tstd.lib.superRaycast{

	public class SuperRaycast : MonoBehaviour {

		public static readonly string GetMouseButtonDown = "GetMouseButtonDown";
		public static readonly string GetMouseButton = "GetMouseButton";
		public static readonly string GetMouseButtonUp = "GetMouseButtonUp";
		public static readonly string GetMouseEnter = "GetMouseEnter";
		public static readonly string GetMouseExit = "GetMouseExit";

		private static SuperRaycast _Instance;

		public static SuperRaycast Instance{

			get{

				if(_Instance == null){

					GameObject go = new GameObject("SuperRaycastGameObject");

					_Instance = go.AddComponent<SuperRaycast>();
				}

				return _Instance;
			}
		}

		private int m_isOpen = 0;
		
		public bool isOpen{
			
			get{
				
				return m_isOpen > 0;
			}
		}
		
		public void SetOpen(bool _b){
			
			m_isOpen = m_isOpen + (_b ? 1 : -1);

			if(m_isOpen == 0){

				if(objs.Count > 0){

					objs.Clear();
				}

			}else if(m_isOpen > 1){
				
				SuperDebug.LogError("SuperGraphicRaycast.SetOpen error!");
			}
		}
		
		public bool filter = false;
		
		public string filterTag;

		private List<GameObject> objs = new List<GameObject>();

		void Update(){
			
			if(isOpen){

				if(Input.GetMouseButtonDown(0)){
					
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					RaycastHit[] hits = Physics.RaycastAll(ray,float.MaxValue,1 << LayerMask.NameToLayer("Default"));

					int i = 0;
					
					foreach(RaycastHit hit in hits){
						
						if(filter && !hit.collider.gameObject.CompareTag(filterTag)){
							
							continue;
						}
						
						objs.Add(hit.collider.gameObject);

						SuperEvent enterEvent = new SuperEvent(GetMouseEnter);
							
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,enterEvent);
							
						SuperEvent e = new SuperEvent(GetMouseButtonDown);
						
						e.data = new object[]{hit,i};
						
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,e);
						
						i++;
					}

				}else if(Input.GetMouseButton(0)){
					
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					RaycastHit[] hits = Physics.RaycastAll(ray,float.MaxValue,1 << LayerMask.NameToLayer("Default"));
					
					List<GameObject> newObjs = new List<GameObject>();

					List<GameObject> enterObjs = new List<GameObject>();
					
					int i = 0;
					
					foreach(RaycastHit hit in hits){
						
						if(filter && !hit.collider.gameObject.CompareTag(filterTag)){
							
							continue;
						}
						
						newObjs.Add(hit.collider.gameObject);
						
						if(!objs.Contains(hit.collider.gameObject)){

							enterObjs.Add(hit.collider.gameObject);
							
//							SuperEvent enterEvent = new SuperEvent(GetMouseEnter);
//							
//							SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,enterEvent);
							
						}else{
							
							objs.Remove(hit.collider.gameObject);
						}
						
						SuperEvent e = new SuperEvent(GetMouseButton);
						
						e.data = new object[]{hit,i};
						
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,e);
						
						i++;
					}
					
					foreach(GameObject go in objs){
						
						SuperEvent exitEvent = new SuperEvent(GetMouseExit);
						
						SuperFunction.Instance.DispatchEvent(go,exitEvent);
					}
					
					objs = newObjs;

					foreach(GameObject go in enterObjs){
						
						SuperEvent enterEvent = new SuperEvent(GetMouseEnter);
						
						SuperFunction.Instance.DispatchEvent(go,enterEvent);
					}

				}else if(Input.GetMouseButtonUp(0)){
					
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					
					RaycastHit[] hits = Physics.RaycastAll(ray,float.MaxValue,1 << LayerMask.NameToLayer("Default"));

					int i = 0;
					
					foreach(RaycastHit hit in hits){
						
						if(filter && !hit.collider.gameObject.CompareTag(filterTag)){
							
							continue;
						}

						SuperEvent e = new SuperEvent(GetMouseButtonUp);
						
						e.data = new object[]{hit,i};
						
						SuperFunction.Instance.DispatchEvent(hit.collider.gameObject,e);
						
						i++;
					}
					
//					foreach(GameObject go in objs){
//						
//						SuperEvent exitEvent = new SuperEvent(GetMouseExit);
//						
//						SuperFunction.Instance.DispatchEvent(go,exitEvent);
//					}
					
					objs.Clear();
				}
			}
		}
	}
}
