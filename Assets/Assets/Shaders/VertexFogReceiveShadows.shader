
Shader "InfiniteCollapse/VertexFogReceiveShadows"
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

		Pass
		{
			Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "AutoLight.cginc"

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
				LIGHTING_COORDS(0, 1)
				float3 diffuse : COLOR0;
				float3 ambient : COLOR1;
				float3 fog : COLOR2;
			};

			void vert(appdata_base input, out VertOut output)
			{
				UNITY_INITIALIZE_OUTPUT(VertOut, output);
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);

				half3 worldNormal = UnityObjectToWorldNormal(input.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				output.diffuse = nl * _LightColor0.rgb;
				output.ambient = ShadeSH9(half4(worldNormal, 1));

				float4 worldSpaceCoord = mul(unity_ObjectToWorld, input.vertex);
				float distToCamera = max(0.0, distance(_WorldSpaceCameraPos, worldSpaceCoord) - _FogStart);
				output.fog.r = clamp(distToCamera / _FogEnd, 0.0, 1.0);

				float currentGradient = worldSpaceCoord.y / _FogHeight + (_GradientCount - _InitialGradient - 1);
				output.fog.g = (1.0 - currentGradient - 1) / _GradientCount;
				output.fog.b = worldSpaceCoord.y / _FogHeight;

				TRANSFER_VERTEX_TO_FRAGMENT(output);
			}


			float4 frag(VertOut input) : COLOR
			{
				float4 color = _MaterialColor;
				float shadow = LIGHT_ATTENUATION(input);
				float3 lighting = input.diffuse * shadow + input.ambient;

				float4 fogColor = tex2D(_FogColor, input.fog.gb);

				color.rgb *= lighting;
				return float4(lerp(color.rgb, fogColor.rgb, input.fog.r), 1.0);
			}

			ENDCG
		}
	}
	Fallback "VertexLit"
}