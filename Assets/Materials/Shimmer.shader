// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Shimmer" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_ShimmerColor ("Shimmer Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;
fixed4 _ShimmerColor;

struct Input {
	float2 uv_MainTex;
	float3 worldPos;
	float3 worldRefl;
    float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	float shimmerValue = IN.worldPos.y;// + (c.r + c.g + c.b);
	float shimmer = saturate(sin(shimmerValue * 3.1415 * 2 + _Time.w * 2));
	half rim = 1 - saturate(dot (normalize(IN.viewDir), o.Normal));
	o.Emission = _ShimmerColor * shimmer * pow(rim,4);
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG
}

Fallback "Legacy Shaders/VertexLit"
}
