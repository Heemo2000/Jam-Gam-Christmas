Shader "Himanshu/ShieldShader"
{
	Properties
	{
		_Color("Color", COLOR) = (1,1,0,1)
		_MainTex("Hex Texture", 2D) = "white" {}
		
		_PulseIntensity("Pulse Intensity", float) = 3.0
		_PulseTimeScale("Hex Pulse Time Scale", float) = 2.0
		_PulsePosScale("Pulse Position Scale", float) = 100.0
		_PulseTexOffsetScale("Pulse Texture Offset Scale", float) = 2.0

		_HexEdgeIntensity("Hex Edge Intensity", float) = 2.0
		_HexEdgeColor("Hex Edge Color", COLOR) = (0,0,0,0)
		_HexEdgeTimeScale("Hex Edge Time Scale", float) = 2.0
		_HexEdgeWidthModifier("Hex Edge Width Modifier",Range(0,1)) = 0.8
		_HexEdgePosScale("Hex Edge Position Scale", float) = 80.0

		_EdgeIntensity("Edge Intensity", float) = 10.0
		_EdgeFalloffExponent("Edge Falloff Exponent", float) = 6.0

		
		_IntersectIntensity("Intersect Intensity", float) = 10.0
		_IntersectFalloffExponent("Intersect Falloff Exponent", float) = 6.0
		
	}
	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Blend SrcAlpha One
		Cull Off 
		
		Pass
		{
			
			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				//Local Position of vertex
				float4 vertexObjPos : TEXCOORD1;
				
				float4 screenPos : TEXCOORD2;
				float depth : TEXCOORD3;
				
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexObjPos : TEXCOORD1;
				
				float4 screenPos : TEXCOORD2;
				float depth : TEXCOORD3;
				
			};

			
			float4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;

			float _PulseIntensity;
			float _PulseTimeScale;
			float _PulsePosScale;
			float _PulseTexOffsetScale;

			float _HexEdgeIntensity;
			float4 _HexEdgeColor;
			float _HexEdgeTimeScale;
			float _HexEdgeWidthModifier;
			float _HexEdgePosScale;

			float _EdgeIntensity;
			float _EdgeFalloffExponent;

			
			sampler2D _CameraDepthNormalsTexture;

			float _IntersectIntensity;
			float _IntersectFalloffExponent;
			
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertexObjPos = v.vertex;

				
				o.screenPos = ComputeScreenPos(o.vertex);
				o.depth = UnityObjectToViewPos(v.vertex) * _ProjectionParams.w; //-mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w;
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//final output
				fixed4 mainTex = tex2D(_MainTex,i.uv);

				fixed4 pulseTex = mainTex.g;
				float horizontalDistance = abs(i.vertexObjPos.x);
				fixed4 pulseTerm = pulseTex * _Color * _PulseIntensity * 
								   abs(sin(_Time.y * _PulseTimeScale - 
								   		   horizontalDistance * _PulsePosScale + 
										   pulseTex.x * _PulseTexOffsetScale));

				fixed4 hexTex = mainTex.r;
				float verticalDistance = abs(i.vertexObjPos.y);
				fixed4 hexEdgeTerm = hexTex * _HexEdgeColor * _HexEdgeIntensity * 
									 max(sin((horizontalDistance + verticalDistance) * _HexEdgePosScale - 
									          _Time.y * _HexEdgeTimeScale) - _HexEdgeWidthModifier, 0.0) *
									 		  1 / (1 - _HexEdgeWidthModifier);

				fixed4 edgeTex = mainTex.b;
				fixed4 edgeTerm = pow(edgeTex.a,_EdgeFalloffExponent) * _Color * _EdgeIntensity;


				
				float diff = //tex2D(_CameraDepthNormalsTexture, i.screenPos.xy).r - i.depth; 
							DecodeFloatRG(tex2D(_CameraDepthNormalsTexture, i.screenPos.xy/ (i.screenPos.w)).zw) - i.depth;
				float intersectGradient = 1 - min(diff/_ProjectionParams.w, 1.0f);

				fixed4 intersectTerm = _Color * pow(intersectGradient, _IntersectFalloffExponent) * _IntersectIntensity;
				return fixed4(_Color.rgb + pulseTerm.rgb + hexEdgeTerm.rgb + edgeTerm + intersectTerm, _Color.a);
				

				
			}

			ENDHLSL
		}
	}
}
