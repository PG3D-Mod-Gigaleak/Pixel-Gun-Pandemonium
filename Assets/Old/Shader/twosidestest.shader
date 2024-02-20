//
// Author:
//   Based on the Unity3D built-in shaders
//   Andreas Suter (andy@edelweissinteractive.com)
//
// Copyright (C) 2012 Edelweiss Interactive (http://edelweissinteractive.com)
//

Shader "Decal/Cutout Bumped Diffuse Two Sided" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	_ColorMultiplier ("Color Multiplier", Color) = (0.25,0.25,0.25,0.25)
}

SubShader {
	Cull Off
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	Offset -1,-1
	
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
fixed4 _ColorMultiplier;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb * _ColorMultiplier.rgb * 4;
	o.Alpha = c.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Decal/Cutout Diffuse"
}
