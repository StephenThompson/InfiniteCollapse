
Shader "InfiniteCollapse/VertexFogUnlit"
{
	Properties
	{
		_MaterialColor("Base Color", Color) = (0, 0, 0, 0)
		_FogColor("Fog Gradients", 2D) = "white" {}
		_GradientCount("Fog Gradient Count", Int) = 8
		_InitialGradient("Initial Fog Gradient", Int) = 0
		_FogHeight("Fog Height", Float) = 30.0
		_FogStart("Linear Fog Start", Range(0, 100)) = 10.0
		_FogEnd("Linear Fog End", Range(0, 100)) = 60.0
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
			float4 _MaterialColor;
			float3 _CameraPos;
			sampler2D _FogColor;
			float _GradientCount;
			int _InitialGradient;
			float _FogHeight;
			float _FogStart;
			float _FogEnd;

			
			struct VertOut {
				float4 pos : SV_POSITION;
				float3 fog : COLOR0;
			};

			void vert(appdata_base input, out VertOut output)
			{
				UNITY_INITIALIZE_OUTPUT(VertOut, output);
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);

				float4 worldSpaceCoord = mul(unity_ObjectToWorld, input.vertex);
				float distToCamera = max(0.0, distance(_WorldSpaceCameraPos, worldSpaceCoord) - _FogStart);
				output.fog.r = clamp(distToCamera / _FogEnd, 0.0, 1.0);

				float currentGradient = worldSpaceCoord.y / _FogHeight + (_GradientCount - _InitialGradient - 1);
				output.fog.g = (1.0 - currentGradient - 1) / _GradientCount;
				output.fog.b = worldSpaceCoord.y / _FogHeight;

			}
						

			float4 frag(VertOut input) : COLOR
			{
				float4 fogColor = tex2D(_FogColor, input.fog.gb);
				return float4(lerp(_MaterialColor.rgb, fogColor.rgb, input.fog.r), 1.0);
			}

		ENDCG
		}
	}
}