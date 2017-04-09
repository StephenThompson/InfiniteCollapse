
Shader "InfiniteCollapse/VertexFogDiffuse"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Base (RGB)", 2D) = "white" {}
		_FogColor("Fog Color", Color) = (0,0,0,0)
		_FogStart("Linear Fog Start", Range(0, 100)) = 0.0
		_FogEnd("Linear Fog End", Range(0, 100)) = 10.0
	}
		SubShader
	{
		
		Pass
		{
			Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			// Set up 
			sampler2D _MainTex;
			float3 _CameraPos;
			float4 _FogColor;
			float _FogStart;
			float _FogEnd;

			struct VertOut {
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float fog : TEXCOOR1;
				fixed3 diffuse : COLOR0;
			};

			VertOut vert(appdata_base input)
			{
				VertOut output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.uv = input.texcoord;

				half3 worldNormal = UnityObjectToWorldNormal(input.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				output.diffuse = nl * _LightColor0;
				output.diffuse.rgb += ShadeSH9(half4(worldNormal, 1));

				float4 worldSpaceCoord = mul(unity_ObjectToWorld, input.vertex);
				float distToCamera = max(0.0, distance(_WorldSpaceCameraPos, worldSpaceCoord) - _FogStart);
				output.fog = clamp(distToCamera / _FogEnd, 0.0, 1.0);

				return output;
			}
						

			fixed4 frag(VertOut input) : COLOR
			{
				fixed4 color = tex2D(_MainTex, input.uv);
				color *= fixed4(input.diffuse, 1.0);
				return fixed4(lerp(color.rgb, _FogColor.rgb, input.fog), 1.0);
			}

		ENDCG
		}
	}
}