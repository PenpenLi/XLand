﻿Shader "Custom/UI/GrayMask_2"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_ShowGrayState("OpenGray",Float) = 0
		
//		_StencilComp ("Stencil Comparison", Float) = 8
//		_Stencil ("Stencil ID", Float) = 0
//		_StencilOp ("Stencil Operation", Float) = 0
//		_StencilWriteMask ("Stencil Write Mask", Float) = 255
//		_StencilReadMask ("Stencil Read Mask", Float) = 255
		
		_ColorMask ("Color Mask", Float) = 15
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent+2" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
//		Stencil
//		{
//			Ref [_Stencil]
//			Comp [_StencilComp]
//			Pass [_StencilOp] 
//			ReadMask [_StencilReadMask]
//			WriteMask [_StencilWriteMask]
//		}
		
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			
			float _UseClipRect;
			float4 _ClipRect;
			
			float _ShowGrayState;
			
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);

				OUT.texcoord = IN.texcoord;
				
				#ifdef UNITY_HALF_TEXEL_OFFSET
				OUT.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,1);
				#endif
				
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : SV_Target
			{
				half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

				float c = 0.299 * color.r + 0.587 * color.g + 0.184 * color.b;
			    
			    color.r = color.r * (1 - _ShowGrayState) + c * _ShowGrayState;
			    color.g = color.g * (1 - _ShowGrayState) + c * _ShowGrayState;
			    color.b = color.b * (1 - _ShowGrayState) + c * _ShowGrayState;
			    
			    color = color * (1 - _UseClipRect) + color * UnityGet2DClipping(IN.worldPosition.xy, _ClipRect) * _UseClipRect;
				    
				return color;
			}
		ENDCG
		}
	}
	
	FallBack "UI/Default"
}
