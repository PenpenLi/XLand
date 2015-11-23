using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using xy3d.tstd.lib.textureFactory;

namespace xy3d.tstd.lib.textureFactory{

	public class ImageScript : MonoBehaviour {
		
		private Image img;
		
		public string imgName;

		void Awake(){

			img = GetComponent<Image>();
		}

		void OnEnable(){

			if(img.sprite == null){
				
				TextureFactory.Instance.SetSprite(img,imgName,false);
			}
		}
	}
}