// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

Shader "InfiniteCollapse/VertexFog"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FogColor("Fog Color", Color) = (0,0,0,0)
		_FogStart("Linear Fog Start", Range(0, 100)) = 0.0
		_FogEnd("Linear Fog End", Range(0, 100)) = 10.0
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			// Set up 
			sampler2D _MainTex;
			float3 _CameraPos;
			float4 _FogColor;
			float _FogStart;
			float _FogEnd;

			struct VertIn {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};
			struct VertOut {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float fog : TEXCOORD1;
			};

			VertOut vert(VertIn input)
			{
				VertOut output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.uv = input.texcoord;

				float4 worldSpaceCoord = mul(unity_ObjectToWorld, input.vertex);
				float distToCamera = max(0.0, distance(_WorldSpaceCameraPos, worldSpaceCoord) - _FogStart);
				output.fog = clamp(distToCamera / _FogEnd, 0.0, 1.0);

				return output;
			}
						

			float4 frag(VertOut input) : COLOR
			{
				float4 tex = tex2D(_MainTex, input.uv);
				return float4(lerp(tex.rgb, _FogColor.rgb, input.fog), 1.0);
			}

		ENDCG
		}
	}
}