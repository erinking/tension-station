Shader "Custom/Diffuse With Glowy Bits" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_DetailColor ("Detail Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Detail ("Detail (RGB)", 2D) = "gray" {}
	_Cutoff ("Alpha Cutoff", Float) = 0.5
}

SubShader {
	Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 200

	CGPROGRAM
	#pragma surface surf Lambert alphatest:_Cutoff

	sampler2D _MainTex;
	sampler2D _Detail;
	fixed4 _Color;
	fixed4 _DetailColor;

	struct Input {
		float2 uv_MainTex;
		float2 uv_Detail;
	};

	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Emission = tex2D(_Detail,IN.uv_Detail).rgb * unity_ColorSpaceDouble.r * _DetailColor;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
}

Fallback "Legacy Shaders/Diffuse"
}